using System;
using System.Text;
using AIMLbot;
using DataAccess;
using Serilog;

namespace Services;

public class GaiaService
{
    private Bot _gaia;
    private User _user;
    
    public GaiaService()
    {
        _gaia = new Bot();
        

        _gaia.LoadSettings(); // This loads the settings from the config folder

        _gaia.LoadAimlFromFiles(); // This loads the AIML files from the aiml folder

        _gaia.IsAcceptingUserInput = false; // This switches off the bot to stop the user entering input while the bot is loading

        _user = new User("Permasteader", _gaia); // This creates a new User called "Username", using the object "AI"'s information.

        _gaia.IsAcceptingUserInput = true; // This switches the user input back on

       
    }

    public string GetResponse(string inputString)
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
                    var searchList = GetSearchResults(inputString.Replace("/s ",""));

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

    public StringBuilder GetSearchResults(string inputString) 
    {
        var rtnList = new StringBuilder();

        rtnList = DataAccess.Local.ObservationRepository.SearchObservations(DataConnection.GetLocalDataSource(), inputString);

        return rtnList; 
    }
}