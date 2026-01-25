using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [ObservableProperty] private string _precipitation = "";
    [ObservableProperty] private string _moonPhase = "";
    [ObservableProperty] private string _moonRise = "";
    [ObservableProperty] private string _moonSet = "";
    [ObservableProperty] private string _moonIllumination = "";
    [ObservableProperty] private string _sunRise = "";
    [ObservableProperty] private string _sunSet = "";
    
    [ObservableProperty] private string _date1 = "";
    [ObservableProperty] private string _avgtempC1 = "";
    [ObservableProperty] private string _maxtempC1 = "";
    [ObservableProperty] private string _mintempC1 = "";
    
    [ObservableProperty] private string _date2 = "";
    [ObservableProperty] private string _avgtempC2 = "";
    [ObservableProperty] private string _maxtempC2 = "";
    [ObservableProperty] private string _mintempC2 = "";
    
    [ObservableProperty] private string _date3 = "";
    [ObservableProperty] private string _avgtempC3 = "";
    [ObservableProperty] private string _maxtempC3 = "";
    [ObservableProperty] private string _mintempC3 = "";
    
    [ObservableProperty] 
    private double[] _tempCSeries; 
    
    [ObservableProperty] 
    private double[] _feelsLikeCSeries; 
    
    [ObservableProperty] 
    private double[] _precipSeries; 
    
    [ObservableProperty] 
    private double[] _windKphSeries; 
    
    [ObservableProperty] 
    private double[] _humiditySeries; 
    

    public ObservableCollection<WeatherModel.Weather> WeatherForecastItems { get; set; } =
        new ObservableCollection<WeatherModel.Weather>();
    
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
        try
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
                    
                    // get the current weather
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
                    Precipitation = ws.ModelRoot!.current_condition[0].precipMM;
                
                    SunRise = ws.ModelRoot!.weather[0].astronomy[0].sunrise;
                    SunSet = ws.ModelRoot!.weather[0].astronomy[0].sunset;
                
                    MoonRise = ws.ModelRoot!.weather[0].astronomy[0].moonrise;
                    MoonSet = ws.ModelRoot!.weather[0].astronomy[0].moonset;
                    MoonIllumination = ws.ModelRoot!.weather[0].astronomy[0].moon_illumination;

                    // get the three day forecast
                    WeatherForecastItems = new ObservableCollection<WeatherModel.Weather>(ws.ModelRoot.weather);

                    var tempCSeriesList = new List<double>();
                    var feelsLikeCSeriesList = new List<double>();
                    var precipSeriesList = new List<double>();
                    var humiditySeriesList = new List<double>();
                    var windSeriesList = new List<double>();
                        
                    if (WeatherForecastItems.Count > 2)
                    {
                        Date1 = WeatherForecastItems[0].date;
                        AvgtempC1 = WeatherForecastItems[0].avgtempC;
                        MaxtempC1 = WeatherForecastItems[0].maxtempC;
                        MintempC1 = WeatherForecastItems[0].mintempC;
                        
                        foreach (var h in WeatherForecastItems[0].hourly)
                        {
                            tempCSeriesList.Add(Convert.ToDouble(h.tempC));
                            feelsLikeCSeriesList.Add(Convert.ToDouble(h.FeelsLikeC));
                            precipSeriesList.Add(Convert.ToDouble(h.precipMM));
                            humiditySeriesList.Add(Convert.ToDouble(h.humidity));
                            windSeriesList.Add(Convert.ToDouble(h.windspeedKmph));
                        }
                        
                        Date2 = WeatherForecastItems[1].date;
                        AvgtempC2 = WeatherForecastItems[1].avgtempC;
                        MaxtempC2 = WeatherForecastItems[1].maxtempC;
                        MintempC2 = WeatherForecastItems[1].mintempC;
                        
                        foreach (var h in WeatherForecastItems[1].hourly)
                        {
                            tempCSeriesList.Add(Convert.ToDouble(h.tempC));
                            feelsLikeCSeriesList.Add(Convert.ToDouble(h.FeelsLikeC));
                            precipSeriesList.Add(Convert.ToDouble(h.precipMM));
                            humiditySeriesList.Add(Convert.ToDouble(h.humidity));
                            windSeriesList.Add(Convert.ToDouble(h.windspeedKmph));
                        }
                    
                        Date3 = WeatherForecastItems[2].date;
                        AvgtempC3 = WeatherForecastItems[2].avgtempC;
                        MaxtempC3 = WeatherForecastItems[2].maxtempC;
                        MintempC3 = WeatherForecastItems[2].mintempC;
                        
                        foreach (var h in WeatherForecastItems[2].hourly)
                        {
                            tempCSeriesList.Add(Convert.ToDouble(h.tempC));
                            feelsLikeCSeriesList.Add(Convert.ToDouble(h.FeelsLikeC));
                            precipSeriesList.Add(Convert.ToDouble(h.precipMM));
                            humiditySeriesList.Add(Convert.ToDouble(h.humidity));
                            windSeriesList.Add(Convert.ToDouble(h.windspeedKmph));
                        }
                        
                        TempCSeries = tempCSeriesList.ToArray();
                        FeelsLikeCSeries = feelsLikeCSeriesList.ToArray();
                        PrecipSeries = precipSeriesList.ToArray();
                        HumiditySeries = humiditySeriesList.ToArray();
                        WindKphSeries = windSeriesList.ToArray();
                    }
                    
                    

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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}