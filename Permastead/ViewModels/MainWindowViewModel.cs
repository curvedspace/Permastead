
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Permastead.ViewModels.Views;
using Permastead.Views;
using Models;

namespace Permastead.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
        
        [ObservableProperty] private ScoreBoard _scoreBoard;
        
        [ObservableProperty] private bool _gaiaOpen;
        
        public GaiaWindow GaiaWindow {
            get;
            set;
        }

        
        [ObservableProperty]
        private string _currentUser;
        
        private ViewModelBase[] Views =
        {
            new HomeViewModel(),
            new AlertsViewModel(),
            new DashboardViewModel(),
            new ConnectionsViewModel(),
            new CyclesViewModel(),
            new FinderViewModel(),
            new ObservationsViewModel(),
            new ToDoViewModel(),
            new EventsViewModel(),
            new ProceduresViewModel(),
            new PlantsViewModel(),
            new SeedsViewModel(),
            new PlannerViewModel(),
            new PlantingsViewModel(),
            new AnimalsViewModel(),
            new HarvestsViewModel(),
            new PreservationViewModel(),
            new InventoryViewModel(),
            new ContactsViewModel(),
            new WeatherViewModel(),
            new SettingsViewModel(),
            new DataViewModel()
        };

        [ObservableProperty] private decimal _levelProgress;

        [ObservableProperty] private int _alertCount;

        [ObservableProperty] 
        private ViewModelBase _currentView;
  
    
        [ObservableProperty] 
        private string _currentViewName;

        [RelayCommand]
        private void OpenHomeView()
        {
            SetupView(ToolbarViews.Home);

            var vm = CurrentView as HomeViewModel;
            
            // when we click on Home, refresh the data
            vm?.RefreshData();

        }


        [RelayCommand]
        private void OpenAlertsView()
        {
            SetupView(ToolbarViews.Alerts);

            var vm = CurrentView as AlertsViewModel;
            
            // when we click on Home, refresh the data
            vm?.RefreshDataCommand.Execute(null);
        }
        
        [RelayCommand]
        private void OpenDashboardView() =>  SetupView(ToolbarViews.Dashboard);
        
        [RelayCommand]
        private void OpenConnectionsView() =>  SetupView(ToolbarViews.Connections);
        
        [RelayCommand]
        private void OpenCyclesView() =>  SetupView(ToolbarViews.Cycles);
        
        [RelayCommand]
        private void OpenFinderView() =>  SetupView(ToolbarViews.Finder);

        [RelayCommand]
        private void OpenObservationView() =>  SetupView(ToolbarViews.Observations);
        
        [RelayCommand]
        private void OpenToDoView() =>  SetupView(ToolbarViews.Actions);
        
        [RelayCommand]
        private void OpenEventsView() =>  SetupView(ToolbarViews.Events);
        
        [RelayCommand]
        private void OpenProceduresView() =>  SetupView(ToolbarViews.Procedures);
        
        [RelayCommand]
        private void OpenPlantsView() =>  SetupView(ToolbarViews.Plants);
        
        [RelayCommand]
        private void OpenSeedsView() =>  SetupView(ToolbarViews.Seeds);
        
        [RelayCommand]
        private void OpenPlantingsView() =>  SetupView(ToolbarViews.Plantings);
        
        [RelayCommand]
        private void OpenAnimalsView() =>  SetupView(ToolbarViews.Animals);
        
        [RelayCommand]
        private void OpenHarvestsView() =>  SetupView(ToolbarViews.Harvests);
        
        [RelayCommand]
        private void OpenPreservationView() =>  SetupView(ToolbarViews.Preservations);
        
        [RelayCommand]
        private void OpenInventoryView() =>  SetupView(ToolbarViews.Inventory);
        
        [RelayCommand]
        private void OpenPeopleView() =>  SetupView(ToolbarViews.People);

        [RelayCommand]
        private void OpenWeatherView()
        {
            // create a new view model so the weather gets updated
            Views[(int)ToolbarViews.Weather] = new WeatherViewModel();
            SetupView(ToolbarViews.Weather);
        }
        
        [RelayCommand]
        private void OpenSettingsView()  =>  SetupView(ToolbarViews.Settings);
        
        [RelayCommand]
        private void OpenDataView()  =>  SetupView(ToolbarViews.Data);
        
        [RelayCommand]
        private void OpenPlannerView()  =>  SetupView(ToolbarViews.Planner);

       
        [RelayCommand]
        private void OpenGaiaView()
        {
            if (_gaiaOpen)
            {
                if (GaiaWindow != null && GaiaWindow.IsLoaded)
                {
                    this.GaiaWindow.Show();
                }
                else
                {
                    GaiaWindow = new GaiaWindow();
                    
                    this.GaiaWindow.Width = 700;
                    this.GaiaWindow.Show();
                }
            }
            else
            {
                if (GaiaWindow != null)
                {
                    this.GaiaWindow.Hide();
                }
            }
        }
        
        /// <summary>
        /// A helper method to select the current view.
        /// </summary>
        /// <param name="view">The view to select.</param>
        private void SetupView(ToolbarViews view)
        {
            CurrentView = Views[(int)view];
            var current = view.ToString();

            switch (current)
            {
                case "Home":
                    CurrentViewName = "Home - details on the past 30 days";
                    break;
                case "Actions":
                    CurrentViewName = "Actions - your current TODO list";
                    break;
                case "Planner":
                    CurrentViewName = "Planner - plan your gardens ahead of time";
                    break;
                case "Observations":
                    CurrentViewName = "Observations - your own journal";
                    break;
                case "Procedures":
                    CurrentViewName = "Procedures - store instructions, procedures, other text data";
                    break;
                case "Weather":
                    CurrentViewName = "Weather - local weather forecast from wttr";
                    break;
                case "People":
                    CurrentViewName = "People - contact data for your network";
                    break;
                case "Cycles":
                    CurrentViewName = "Cycles - a visual representation of time cycles ";
                    break;
                case "Dashboard":
                    CurrentViewName = "Dashboard - visual representation of yearly data ";
                    break;
                case "Plants":
                    CurrentViewName = "Plants - your own plant database ";
                    break;
                case "Plantings":
                    CurrentViewName = "Plantings - what you have planted and where ";
                    break;
                case "Seeds":
                    CurrentViewName = "Starters - seeds and purchased plants ";
                    break;
                case "Animals":
                    CurrentViewName = "Animals - livestock and pets ";
                    break;
                case "Harvests":
                    CurrentViewName = "Harvests - what you have harvested and when ";
                    break;
                case "Preservations":
                    CurrentViewName = "Preservations - food and medicine preservations ";
                    break;
                case "Inventory":
                    CurrentViewName = "Inventory - a list of what you currently own ";
                    break;
                case "Finder":
                    CurrentViewName = "Finder - search for keywords in your data ";
                    break;
                default:
                    CurrentViewName = current;
                    break;
            }
            
            
            AlertCount = AppSession.Instance.AlertManager.Count;
            LevelProgress = ScoreBoard.LevelProgress *  100;
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
            CurrentUser = AppSession.Instance.CurrentUser.FirstName;
            CurrentViewName = "Home";
            
            ScoreBoard = AppSession.Instance.CurrentScoreboard;
            
            GaiaWindow = new GaiaWindow();
            
            LevelProgress = ScoreBoard.LevelProgress *  100;
            AlertCount = AppSession.Instance.AlertManager.Count;
        }
        
     }
    
    public enum ToolbarViews
    {
        Home,
        Alerts,
        Dashboard,
        Connections,
        Cycles,
        Finder,
        Observations,
        Actions,
        Events,
        Procedures,
        Plants,
        Seeds,
        Planner,
        Plantings,
        Animals,
        Harvests,
        Preservations,
        Inventory,
        People,
        Weather,
        Settings,
        Data,
        Gaia
    }
}