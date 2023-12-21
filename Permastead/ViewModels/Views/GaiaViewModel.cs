using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        _gaia = new Services.GaiaService();

        //Response = _gaia.GetResponse("Hello");
        
        try
        {
            var t = new Task(GetWeatherAsync);
            t.Start();
            t.Wait();

            if (string.IsNullOrEmpty(WeatherForecast))
            {
                RequestResponses.Add(new RequestResponse() { Request = "Hello", Response = "Hello, I am Gaia." });
            }
            else
            {
                RequestResponses.Add(new RequestResponse() { Request = "The weather please?", Response = WeatherForecast });
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

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
    
    bool CanSendRequest(object parameter)
    {
        return Response != null;
    }
    
    
    
}

public class RequestResponse
{

    public string Request { get; set; }
    public string Response { get; set; }

    public string FullText => DateTime.Now.ToShortTimeString() + ": " + Request + '\n' + "     " + Response;

}