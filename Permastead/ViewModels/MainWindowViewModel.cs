
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
        
        public decimal LevelProgress {get => _scoreBoard.LevelProgress *  100;}

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
        private void OpenWeatherView()  =>  SetupView(ToolbarViews.Weather);
        
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
            CurrentViewName = view.ToString();
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
            CurrentUser = AppSession.Instance.CurrentUser.FirstName;
            CurrentViewName = "Home";
            
            ScoreBoard = AppSession.Instance.CurrentScoreboard;
            
            GaiaWindow = new GaiaWindow();
        }
        
     }
    
    public enum ToolbarViews
    {
        Home,
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