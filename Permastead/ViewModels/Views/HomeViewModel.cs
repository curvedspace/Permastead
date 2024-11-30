
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;

using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Measure;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

    public partial class HomeViewModel : ViewModelBase
    {
        public IEnumerable<ISeries> Series { get; set; }
        
        public IEnumerable<ISeries> SeriesStats { get; set; }
        public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }
        private NeedleVisual Needle { get; set; }
        
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
        private ObservableCollection<Harvest> _harvests = new ObservableCollection<Harvest>();
        
        [ObservableProperty] 
        private ObservableCollection<ToDo> _toDos = new ObservableCollection<ToDo>();
        
        [ObservableProperty]
        private string? _statistics;
        
        [ObservableProperty]
        private double _totalScore;

        [ObservableProperty] private string _CurrentDateDisplay = DateTime.Now.ToLongDateString();

        [ObservableProperty] private string _weatherForecast = "Weather Unknown";
        
        public ObservableValue PlantingsValue { get; set; } = new ObservableValue(0);
        public ObservableValue HarvestsValue { get; set; } = new ObservableValue(0);
        public ObservableValue InventoryValue { get; set; } = new ObservableValue(0);
        public ObservableValue ObservationsValue { get; set; } = new ObservableValue(0);

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
            
            var sectionsOuter = 130;
            var sectionsWidth = 20;

            Needle = new NeedleVisual
            {
                Value = 45
            };

            Series = GaugeGenerator.BuildAngularGaugeSections(
                new GaugeItem(40, s => SetStyle(sectionsOuter, sectionsWidth, s)),
                new GaugeItem(20, s => SetStyle(sectionsOuter, sectionsWidth, s)),
                new GaugeItem(40, s => SetStyle(sectionsOuter, sectionsWidth, s)));

            VisualElements =
            [
                new AngularTicksVisual
                {
                    LabelsSize = 16,
                    LabelsOuterOffset = 15,
                    OuterOffset = 65,
                    TicksLength = 20
                },
                Needle
            ];
            
            OnPropertyChanged(nameof(Series));
            
         SeriesStats  =
            GaugeGenerator.BuildSolidGauge(
                new GaugeItem(HarvestsValue, series => SetStyleStats("Harvests", series)),
                new GaugeItem(PlantingsValue, series => SetStyleStats("Plantings", series)),
                new GaugeItem(InventoryValue, series => SetStyleStats("Inventory", series)),
                new GaugeItem(ObservationsValue, series => SetStyleStats("Observations", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.InnerRadius = 20;
                }));
            
            this.QuoteViewModel.Quote.Description = "This is a test quote";
            this.QuoteViewModel.Quote.AuthorName = "Anonymous";
            
            
            PlantingYearStartDate = new DateTime(DateTime.Today.Year, 1,1);
            PlantingYearEndDate = new DateTime(DateTime.Today.Year, 12,31);

            // make sure we have an instance instantiated so ServiceMode is set
            AppSession.Instance.ToString();
            
            //get observations
            Observations = new ObservableCollection<Observation>(Services.ObservationsService.GetObservations(AppSession.ServiceMode));
            
            var scoreBoard = ScoreBoardService.ComputeTotalScore(AppSession.ServiceMode);
            AppSession.Instance.CurrentScoreboard = scoreBoard;

            this.Statistics = scoreBoard.ToString();
            this.TotalScore = (double)scoreBoard.TotalScore;

            // this should represent how close, in a scale of 1-100, that we are to the next level
            TotalScoreNormalized = Math.Round((this._totalScore - scoreBoard.LevelMin) / (scoreBoard.LevelMax - scoreBoard.LevelMin),4);

           
            ObservationsToActionRatio = scoreBoard.ActionsToObservationsRatio;
            
            // get other data
            Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode));
            InventoryItems = new ObservableCollection<Inventory>(InventoryService.GetAllInventory(AppSession.ServiceMode));
            SeedPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
            Plants = new ObservableCollection<Plant>(PlantingsService.GetPlants(AppSession.ServiceMode));
            People = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            ToDos = new ObservableCollection<ToDo>(ToDoService.GetAllToDos(AppSession.ServiceMode));
            
            GetQuote();
            
            var startDateWindow = DateTime.Today.AddDays(-30);
            this.PlantingsValue.Value = 0;
            this.HarvestsValue.Value = 0;
            this.InventoryValue.Value = 0;
            this.ObservationsValue.Value = 0;
            
            Needle.Value = Convert.ToDouble(ObservationsToActionRatio);
            
            //compute the observations count
            foreach (var obs in Observations)
            {
                if (obs.StartDate >= startDateWindow) this.ObservationsValue.Value += 1;
            }
            
            //compute the actions count
            foreach (var inv in InventoryItems)
            {
                if (inv.StartDate >= startDateWindow) this.InventoryValue.Value += 1;
            }
            
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

                    if (p.StartDate >= startDateWindow)
                    {
                        this.PlantingsValue.Value += 1;
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
        
        private static void SetStyle(
            double sectionsOuter, double sectionsWidth, PieSeries<ObservableValue> series)
        {
            series.OuterRadiusOffset = sectionsOuter;
            series.MaxRadialColumnWidth = sectionsWidth;
        }
        
        public static void SetStyleStats(string name, PieSeries<ObservableValue> series)
        {
            series.Name = name;
            series.DataLabelsPosition = PolarLabelsPosition.Start;
            series.DataLabelsFormatter =
                point => $"{point.Coordinate.PrimaryValue} {point.Context.Series.Name}";
            series.InnerRadius = 20;
            series.RelativeOuterRadius = 8;
            series.RelativeInnerRadius = 8;
        }

    }

