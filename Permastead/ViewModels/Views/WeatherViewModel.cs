using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class WeatherViewModel : ViewModelBase
{
    [ObservableProperty] private string _weatherForecast = "Weather Unknown";

    [ObservableProperty] private string _weatherTimestamp = "";
    [ObservableProperty] private string _weatherLocation = "";
    [ObservableProperty] private string _weatherStatus = "";
    [ObservableProperty] private string _temperature = "";
    [ObservableProperty] private string _humidity = "";
    [ObservableProperty] private string _cloudClover = "";
    [ObservableProperty] private string _moonPhase = "";
    
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

                WeatherTimestamp = results.ObservationTime.ToString(CultureInfo.CurrentCulture);
                WeatherLocation = "Current weather for " + city.Name + ", " + city.Country;
                Temperature = results.Temperature.ToString(CultureInfo.CurrentCulture);
                Humidity = results.Humidity.ToString(CultureInfo.CurrentCulture);
                CloudClover = results.CloudCover.ToString(CultureInfo.CurrentCulture);
                MoonPhase = results.MoonPhase.ToString(CultureInfo.CurrentCulture);
                WeatherStatus = results.WeatherStateAlias.ToString(CultureInfo.CurrentCulture);
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