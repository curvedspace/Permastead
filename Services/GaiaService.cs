using System;
using System.Collections.ObjectModel;
using System.Text;
using AIMLbot;
using DataAccess;
using Serilog;

namespace Services;

public class GaiaService
{
    private Bot _gaia;
    private User _user;
    
    public ObservableCollection<RequestResponse> RequestResponses = new ObservableCollection<RequestResponse>();
    
    public GaiaService(ServiceMode mode)
    {
        _gaia = new Bot();
        

        _gaia.LoadSettings(); // This loads the settings from the config folder

        _gaia.LoadAimlFromFiles(); // This loads the AIML files from the aiml folder

        _gaia.IsAcceptingUserInput = false; // This switches off the bot to stop the user entering input while the bot is loading

        _user = new User("Permasteader", _gaia); // This creates a new User called "Username", using the object "AI"'s information.

        _gaia.IsAcceptingUserInput = true; // This switches the user input back on
        
        AddResponse("Hello there, welcome to Permastead.");
        AddResponse("Quote of the day: " + '\n' + Services.QuoteService.GetRandomQuote(mode).ToString());

        var updateBuilder = new StringBuilder();
        var updates = new ObservableCollection<string>(ScoreBoardService.CheckForNewToDos(mode));
        var upcomingTodos = ToDoService.GetUpcomingToDos(mode, 3);

        if (upcomingTodos.Count > 0)
        {
            updateBuilder.AppendLine("Here are your upcoming events:");
            foreach (var t in upcomingTodos)
            {
                updateBuilder.AppendLine(t.Description + " (" + t.DueDate.Date.ToShortDateString() + ", " +
                                         t.ToDoStatus.Description + ").");
            }
        }
        
        foreach (var t in updates)
        {
            updateBuilder.AppendLine(t);
        }
        
        if (upcomingTodos.Count == 0)
        {
            AddResponse("I just checked the database and you have no upcoming events.");
        }
        else
        {
            AddResponse(updateBuilder.ToString());
        }
    }

    public string GetResponse(string inputString, ServiceMode mode)
    {
        if (inputString.StartsWith("/")) 
        {
            switch (inputString)
            {
                case "/help":
                    return "You can ask me questions, but I also know the following commands: " + '\n' +
                           "/help - Displays a list of commands" + '\n' +
                           "/s - will search observations for a word.";
                    break;
                
                default:
                    var searchList = GetSearchResults(inputString.Replace("/s ",""), mode);

                    return searchList.ToString();

                    break;
            }
        }
        else 
        { 
            Request r = new Request(inputString, _user, _gaia); // This generates a request using the Console's ReadLine function to get text from the console, the user and the AI object's.
            Result res = _gaia.Chat(r); // This sends the request off to the object AI to get a reply back based of the AIML file's.

            Log.Logger.Information("Gaia request: {Request}, response: {Response}", r.rawInput, res.Output);
        
            return res.Output;
        }
    }

    public StringBuilder GetSearchResults(string inputString, ServiceMode mode) 
    {
        var rtnList = new StringBuilder();

        if (mode == ServiceMode.Local)
        {
            rtnList = DataAccess.Local.ObservationRepository.SearchObservations(DataConnection.GetLocalDataSource(), inputString);
        }
        else
        {
            rtnList = DataAccess.Server.ObservationRepository.SearchObservations(DataConnection.GetLocalDataSource(), inputString);
        }
        
        return rtnList; 
    }
    
    public void AddRequestResponse(RequestResponse rr)
    {
        RequestResponses.Add(rr);
    }
    
    public void AddResponse(string response)
    {
        RequestResponses.Add(new RequestResponse() { Request = "", Response = response });
    }
}

public class RequestResponse
{

    public string Request { get; set; }
    public string Response { get; set; }
    
    public bool HasResponse  => !string.IsNullOrEmpty(Response);
    public bool HasRequest => !string.IsNullOrEmpty(Request);

    public string FullText => DateTime.Now.ToShortTimeString() + ": " + Request + '\n' + "     " + Response;

}