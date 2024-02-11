using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class WeatherViewModel : ViewModelBase
{
    [ObservableProperty] private string _weatherForecast = "Weather Unknown";
    
    public WeatherViewModel()
    {
        try
        {
            var t = new Task(GetWeatherAsync);
            t.Start();
            t.Wait();

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
        var location = SettingsService.GetSettingsForCode("LOC", AppSession.ServiceMode);
        var ctry = SettingsService.GetSettingsForCode("CTRY", AppSession.ServiceMode);

        if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(ctry))
        {
            var city = new City(location, ctry, 0, 0, "");

            try
            {
                var results = await ws.UpdateWeather(city);
                WeatherForecast = "Current Weather " + " as of " + results.ObservationTime + " for " + city.Name + ", " + city.Country + ": " + 
                                  results.WeatherStateAlias + 
                                  ", Cloud Cover: " + results.CloudCover + 
                                  ", Temperature: " + results.Temperature + 
                                  ", Humidity: " + results.Humidity +
                                  ", Moon Phase: " + results.MoonPhase;
                
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
}