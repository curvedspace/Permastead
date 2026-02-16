
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;

using CommunityToolkit.Mvvm.Input;
using DataAccess;
using LiveChartsCore.Measure;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Services;
using Ursa.Controls;

namespace Permastead.ViewModels.Views;

    public partial class HomeViewModel : ViewModelBase
    {
        public IEnumerable<ISeries> Series { get; set; }
        
        public IEnumerable<ISeries> SeriesStats { get; set; }
        
        public double? SeriesStatsMaxValue { get; set; }
        
        // public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }
        private NeedleVisual Needle { get; set; }
        
        [ObservableProperty] private long _inventoryCount;
        
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
        private ObservableCollection<FoodPreservation> _preservations = new ObservableCollection<FoodPreservation>();
        
        [ObservableProperty]
        private string? _statistics;
        
        [ObservableProperty]
        private string? _allTimeStatistics;
        
        [ObservableProperty]
        private double _totalScore;
        
        [ObservableProperty]
        private string _upcomingEvents;

        [ObservableProperty] private string _currentEvents;

        [ObservableProperty] private string _CurrentDateDisplay = DateTime.Now.ToLongDateString();

        [ObservableProperty] private string _weatherForecast = "Weather Unknown";
        
        public WindowToastManager? ToastManager { get; set; }

        public ObservableValue PlantsValue { get; set; } = new ObservableValue(0);
        public ObservableValue StartersValue { get; set; } = new ObservableValue(0);
        public ObservableValue PlantingsValue { get; set; } = new ObservableValue(0);
        public ObservableValue HarvestsValue { get; set; } = new ObservableValue(0);
        public ObservableValue InventoryValue { get; set; } = new ObservableValue(0);
        public ObservableValue FoodPreservationValue { get; set; } = new ObservableValue(0);

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
        
        //message box data
        private readonly string _shortMessage = "Are you sure you want to remove this quote?";
        private string _message;
        private string? _title = "Deletion Confirmation";
    
        public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
        private MessageBoxIcon _selectedIcon;
        public MessageBoxIcon SelectedIcon
        {
            get => _selectedIcon;
            set => SetProperty(ref _selectedIcon, value);
        }

        private MessageBoxResult _result;
        public MessageBoxResult Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
    
        public ICommand YesNoCommand { get; set; }

        [RelayCommand]
        private void AddQuote()
        {
            var q = new Quote();
            var win = new QuoteWindow();
            var vm = new QuoteWindowViewModel(q, this);
            
            win.DataContext = vm;
        
            win.Topmost = true;
            win.Width = 800;
            win.Height = 330;
            win.Opacity = 0.95;
            win.Title = "New Quote";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
            win.Show();
        }
        
        [RelayCommand]
        private async void DeleteQuote()
        {
            var q = this.QuoteViewModel.Quote;

            try
            {
                if (q != null)
                {
                    await OnYesNoAsync();

                    if (Result == MessageBoxResult.Yes)
                    {

                        q.EndDate = DateTime.Today;
                        if (QuoteService.DeleteQuote(AppSession.ServiceMode, q))
                        {
                            ToastManager?.Show(new Toast("Quote record has been removed."));
                        }
                        else
                        {
                            ToastManager?.Show(new Toast("Sorry, there was a problem removing the quote."));
                        }
                    }

                }
            }
            finally
            {
                
            }
            
        }
        
        public HomeViewModel()
        {
           
            _upcomingEvents = "I have checked the database and you have no upcoming events.";

            
            this.QuoteViewModel.Quote.Description = "This is a test quote";
            this.QuoteViewModel.Quote.AuthorName = "Anonymous";
            
            
            PlantingYearStartDate = new DateTime(DateTime.Today.Year, 1,1);
            PlantingYearEndDate = new DateTime(DateTime.Today.Year, 12,31);

            // make sure we have an instance instantiated so ServiceMode is set
            AppSession.Instance.ToString();
            
            RefreshData();
            GetQuote();
            
            YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
            Icons = new ObservableCollection<MessageBoxIcon>(
                Enum.GetValues<MessageBoxIcon>());
            SelectedIcon = MessageBoxIcon.Question;
            _message = _shortMessage;
            
        }

        public void RefreshData()
        {
            var sectionsOuter = 130;
            var sectionsWidth = 20;
            
            //get observations
            Observations = new ObservableCollection<Observation>(Services.ObservationsService.GetObservations(AppSession.ServiceMode));
            
            var scoreBoard = ScoreBoardService.ComputeTotalScore(AppSession.ServiceMode);
            AppSession.Instance.CurrentScoreboard = scoreBoard;

            this.Statistics = scoreBoard.ToString();
            this.TotalScore = (double)scoreBoard.TotalScore;

            // this should represent how close, in a scale of 1-100, that we are to the next level
            TotalScoreNormalized = Math.Round((this._totalScore - scoreBoard.LevelMin) / (scoreBoard.LevelMax - scoreBoard.LevelMin),4);

            var currentEvents = Services.EventsService.GetCurrentHoliday(AppSession.ServiceMode);

            if (currentEvents != null && currentEvents.Count > 0)
            {
                CurrentDateDisplay = DateTime.Now.ToLongDateString();
                CurrentEvents = currentEvents[0].Description.ToString();
                CurrentDateDisplay = CurrentDateDisplay + " (" + CurrentEvents + ")";
            }
            else
            {
                CurrentEvents = string.Empty;
            }
            
            ObservationsToActionRatio = scoreBoard.ActionsToObservationsRatio;
            
            // get other data
            Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode));
            InventoryItems = new ObservableCollection<Inventory>(InventoryService.GetAllInventory(AppSession.ServiceMode));
            SeedPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
            Plants = new ObservableCollection<Plant>(PlantingsService.GetPlants(AppSession.ServiceMode));
            People = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            ToDos = new ObservableCollection<ToDo>(ToDoService.GetAllToDos(AppSession.ServiceMode));
            Harvests = new ObservableCollection<Harvest>(HarvestService.GetAllHarvests(AppSession.ServiceMode));
            Preservations = new ObservableCollection<FoodPreservation>(FoodPreservationService.GetAll(AppSession.ServiceMode));
            
            var startDateWindow = DateTime.Today.AddDays(-30);
            
            this.PlantingsValue.Value = 0;
            this.HarvestsValue.Value = 0;
            this.InventoryValue.Value = 0;
            this.FoodPreservationValue.Value = 0;
            this.StartersValue.Value = 0;
            this.PlantsValue.Value = 0;
            
            InventoryCount = InventoryItems.Count;

            AllTimeStatistics = "                                    " +  
                                "***  Actions: " + ToDos.Count + "   " +
                                "Observations: " + Observations.Count + "   " +
                                "People: " + People.Count + "   " +
                                "Plants: " + Plants.Count + "   " +
                                "Starters: " + SeedPackets.Count + "   " +
                                "Plantings: " + Plantings.Count + "   " +
                                "Harvests: " + Harvests.Count + "   " +
                                "Preservations: " + Preservations.Count + "   " +
                                "Inventory: " + InventoryItems.Count + "   ***";
                                
            //compute the plants count
            foreach (var plant in Plants)
            {
                if (plant.StartDate >= startDateWindow) this.PlantsValue.Value += 1;
            }
            
            //compute the starters count
            foreach (var seed in SeedPackets)
            {
                if (seed.StartDate >= startDateWindow) this.StartersValue.Value += 1;
            }
            
            //compute the observations count
            foreach (var pres in Preservations)
            {
                if (pres.StartDate >= startDateWindow) this.FoodPreservationValue.Value += 1;
            }
            
            //compute the actions count
            foreach (var inv in InventoryItems)
            {
                if (inv.CreationDate >= startDateWindow) this.InventoryValue.Value += 1;
            }
            
            //compute the harvests count
            foreach (var item in Harvests)
            {
                if (item.HarvestDate >= startDateWindow) this.HarvestsValue.Value += 1;
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

            SeriesStatsMaxValue = FoodPreservationValue.Value;
            
            if (PlantsValue.Value > SeriesStatsMaxValue) SeriesStatsMaxValue = PlantsValue.Value;
            if (StartersValue.Value > SeriesStatsMaxValue) SeriesStatsMaxValue = StartersValue.Value;
            if (PlantingsValue.Value > SeriesStatsMaxValue) SeriesStatsMaxValue = PlantingsValue.Value;
            if (InventoryValue.Value > SeriesStatsMaxValue) SeriesStatsMaxValue = InventoryValue.Value;
            if (HarvestsValue.Value > SeriesStatsMaxValue) SeriesStatsMaxValue = HarvestsValue.Value;
            
            TotalPlantStats = "Total: " + TotalPlantings + " Harvested: " + TotalHarvestedPlants + " Failed: " +
                              (TotalPlantings - SuccessfulPlantings);

            _upcomingEvents = AppSession.Instance.GaiaService.InitialWelcomeMessage;

            Needle = new NeedleVisual
            {
                Value = 45
            };

            Series = GaugeGenerator.BuildAngularGaugeSections(
                new GaugeItem(40, s => SetStyle(sectionsOuter, sectionsWidth, s)),
                new GaugeItem(20, s => SetStyle(sectionsOuter, sectionsWidth, s)),
                new GaugeItem(40, s => SetStyle(sectionsOuter, sectionsWidth, s)));

            // VisualElements =
            // [
            //     new AngularTicksVisual
            //     {
            //         LabelsSize = 16,
            //         LabelsOuterOffset = 15,
            //         OuterOffset = 65,
            //         TicksLength = 20
            //     },
            //     Needle
            // ];
            
            OnPropertyChanged(nameof(Series));
            
            SeriesStats  =
                GaugeGenerator.BuildSolidGauge(
                    new GaugeItem(PlantsValue, series => SetStyleStats("Plants", series)),
                    new GaugeItem(StartersValue, series => SetStyleStats("Starters", series)),
                    new GaugeItem(PlantingsValue, series => SetStyleStats("Plantings", series)),
                    new GaugeItem(HarvestsValue, series => SetStyleStats("Harvests", series)),
                    new GaugeItem(FoodPreservationValue, series => SetStyleStats("Preservations", series)),
                    new GaugeItem(InventoryValue, series => SetStyleStats("Inventory", series)),
                    new GaugeItem(GaugeItem.Background, series =>
                    {
                        series.InnerRadius = 20;
                    }));
            
            Needle.Value = Convert.ToDouble(ObservationsToActionRatio);
            
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
                    
                    var alert = new AlertItem() 
                    {
                        Code = "WEATHER", Description = "Current Temperature",
                        Comment = "Temperature: " + results.Temperature
                    };
                    AppSession.Instance.AlertManager.AddAlertIfNotFound(alert);
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

        private async Task OnYesNoAsync()
        {
            await Show(MessageBoxButton.YesNo);
        }
    
        private async Task Show(MessageBoxButton button)
        {
            Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
        }
        
    }

