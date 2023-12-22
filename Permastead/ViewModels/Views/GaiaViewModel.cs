using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class GaiaViewModel : ViewModelBase
{
    [ObservableProperty]
    private long _responseCount;
    
    [ObservableProperty]
    private string _request = string.Empty;
    
    [ObservableProperty]
    private string _response = string.Empty;

    private Services.GaiaService _gaia;
    
    [ObservableProperty] private string _weatherForecast = "Weather Unknown";

    [ObservableProperty]
    private ObservableCollection<RequestResponse> _RequestResponses = new ObservableCollection<RequestResponse>();
    

    public GaiaViewModel()
    {
        _gaia = AppSession.GaiaService;
        //_gaia = new Services.GaiaService();
        
        AddResponse("Hello there, welcome to Permastead.");
        AddResponse("Quote of the day: " + Services.QuoteService.GetRandomQuote(AppSession.ServiceMode).ToString());
        
        var updates = new ObservableCollection<string>(ScoreBoardService.CheckForNewToDos(ServiceMode.Local));

        var upcomingTodos = Services.ToDoService.GetUpcomingToDos(ServiceMode.Local, 3);

        var updateBuilder = new StringBuilder();

        if (upcomingTodos.Count > 0)
        {
            updateBuilder.AppendLine("Here are your upcoming events:");
        }
        
        foreach (var t in upcomingTodos)
        {
            updateBuilder.AppendLine(t.Description + " (" + t.DueDate.Date.ToShortDateString() + ", " +
                                     t.ToDoStatus.Description + ".");
        }
        
        if (upcomingTodos.Count == 0)
        {
            AddResponse("I just checked the database and you have no upcoming events.");
        }
        else
        {
            AddResponse(updateBuilder.ToString());
        }

        
        //Response = _gaia.GetResponse("Hello");
        
        // try
        // {
        //     var t = new Task(GetWeatherAsync);
        //     t.Start();
        //     t.Wait();
        //
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }
        
        // if (string.IsNullOrEmpty(WeatherForecast))
        // {
        //     RequestResponses.Add(new RequestResponse() { Request = "Hello", Response = "Hello, I am Gaia." });
        // }
        // else
        // {
        //     RequestResponses.Add(new RequestResponse() { Request = "The weather please?", Response = WeatherForecast });
        // }

    }  
    
    async void GetWeatherAsync()
    {
        var ws = new Services.WeatherService();
        //var city = new City("Halifax", "Canada", 44.6475, -63.5906, "CA");
            
        //get location from settings
        var location = SettingsService.GetSettingsForCode("LOC");
        var ctry = SettingsService.GetSettingsForCode("CTRY");

        if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(ctry))
        {
            var city = new City(location, ctry, 0, 0, "");

            try
            {
                var results = await ws.UpdateWeather(city);
                WeatherForecast = "Current Weather for " + city.Name + ", " + city.Country + ": " + results.WeatherStateAlias + ", Temperature: " + results.Temperature + ", Humidity: " + results.Humidity;
                Console.WriteLine(WeatherForecast);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                WeatherForecast = "Unable to get weather data.";
            }
        }
        else
        {
            WeatherForecast = "";
        }
            
    }

    [RelayCommand]
    private void SendRequest()
    {
        
        Response = _gaia.GetResponse(Request);
        ResponseCount++;

        if (string.IsNullOrEmpty(Response))
        {
            Response = "Sorry, not sure how to answer.";
            RequestResponses.Add(new RequestResponse() { Request = _request, Response = _response });
        }
        else
        {
            RequestResponses.Add(new RequestResponse() { Request = _request, Response = _response });
        }

        Request = string.Empty;

    }

    public void AddResponse(string response)
    {
        RequestResponses.Add(new RequestResponse() { Request = "", Response = response });
    }
    
    bool CanSendRequest(object parameter)
    {
        return Response != null;
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