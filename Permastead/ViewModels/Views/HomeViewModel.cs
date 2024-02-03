
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

using Models;
using Services;

namespace Permastead.ViewModels.Views;

    public partial class HomeViewModel : ViewModelBase
    {
        public QuoteViewModel QuoteViewModel { get; set; } = new QuoteViewModel();

        [ObservableProperty] 
        private ObservableCollection<Observation> _observations = new ObservableCollection<Observation>();
        
        [ObservableProperty] 
        private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
        
        [ObservableProperty] 
        private ObservableCollection<Inventory> _inventoryItems = new ObservableCollection<Inventory>();
        
        [ObservableProperty] 
        private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
        
        [ObservableProperty] 
        private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
        
        [ObservableProperty] 
        private ObservableCollection<Person> _people = new ObservableCollection<Person>();
        
        [ObservableProperty]
        private string? _statistics;
        
        [ObservableProperty]
        private double _totalScore;

        [ObservableProperty] private string _CurrentDateDisplay = DateTime.Now.ToLongDateString();

        [ObservableProperty] private string _weatherForecast = "Weather Unknown";

        public string GrowingSeason => "Growing Season: " + PlantingYearEndDate.Year.ToString();

        public string HarvestedPlants => "Harvested: " + TotalHarvestedPlants.ToString();
        
        public string TotalScoreFormatted => TotalScore.ToString("F1");
        
        [ObservableProperty]
        private double _totalScoreNormalized;
        
        [ObservableProperty]
        private decimal _observationsToActionRatio;

        private DateTime PlantingYearStartDate;
        
        private DateTime PlantingYearEndDate;

        [ObservableProperty] private long _totalPlantings;
        [ObservableProperty] private long _successfulPlantings;
        [ObservableProperty] private long _totalHarvestedPlants;
        [ObservableProperty] private string _totalPlantStats;
        
        public HomeViewModel()
        {
            this.QuoteViewModel.Quote.Description = "This is a test quote";
            this.QuoteViewModel.Quote.AuthorName = "Anonymous";
            
            PlantingYearStartDate = new DateTime(DateTime.Today.Year, 1,1);
            PlantingYearEndDate = new DateTime(DateTime.Today.Year, 12,31);
            
            //get observations
            Observations = new ObservableCollection<Observation>(Services.ObservationsService.GetObservations(AppSession.ServiceMode));
            
            var scoreBoard = ScoreBoardService.ComputeTotalScore(AppSession.ServiceMode);
            AppSession.Instance.CurrentScoreboard = scoreBoard;

            this.Statistics = scoreBoard.ToString();
            this.TotalScore = (double)scoreBoard.TotalScore;

            // this should represent how close, in a scale of 1-100, that we are to the next level
            TotalScoreNormalized = Math.Round((this._totalScore - scoreBoard.LevelMin) / (scoreBoard.LevelMax - scoreBoard.LevelMin),4);

            decimal ratio = 0;

            if (scoreBoard.Actions > 0)
            {
                ratio = (scoreBoard.Observations / scoreBoard.Actions);
            }
            
            ObservationsToActionRatio = ratio;
            
            // get other data
            Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode));
            InventoryItems = new ObservableCollection<Inventory>(InventoryService.GetAllInventory(AppSession.ServiceMode));
            SeedPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
            Plants = new ObservableCollection<Plant>(PlantingsService.GetPlants(AppSession.ServiceMode));
            People = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));

            //compute the success rate for the current growing year
            foreach (var p in Plantings)
            {
                if (p.EndDate > PlantingYearStartDate)
                {
                    TotalPlantings++;

                    if (p.State.Code != "DEAD")
                    {
                        SuccessfulPlantings++;
                    }

                    if (p.State.Code == "H")
                    {
                        TotalHarvestedPlants++;
                    }
                }
            }

            TotalPlantStats = "Total: " + TotalPlantings + " Harvested: " + TotalHarvestedPlants + " Failed: " +
                              (TotalPlantings - SuccessfulPlantings);

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

        public void GetQuote()
        {
            this.QuoteViewModel.Quote = Services.QuoteService.GetRandomQuote(AppSession.ServiceMode);
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
                    WeatherForecast = "Current Weather " + " as of " + results.ObservationTime + " for " + city.Name + ", " + city.Country + ": " + results.WeatherStateAlias + ", Temperature: " + results.Temperature + ", Humidity: " + results.Humidity;
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

    }

