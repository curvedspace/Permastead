using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Views;
using Permastead.Views;

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
            new FinderViewModel(),
            new ObservationsViewModel(),
            new ToDoViewModel(),
            new EventsViewModel(),
            new InventoryViewModel(),
            new ProceduresViewModel(),
            new PlantsViewModel(),
            new SeedsViewModel(),
            new PlantingsViewModel(),
            new AnimalsViewModel(),
            new HarvestsViewModel(),
            new PreservationViewModel(),
            new ContactsViewModel(),
            new WeatherViewModel(),
            new SettingsViewModel()
        };
        
        public decimal LevelProgress {get => _scoreBoard.LevelProgress *  100;}

        [ObservableProperty] 
        private ViewModelBase _CurrentView;
  
    
        [ObservableProperty] 
        private string _CurrentViewName;

        [RelayCommand]
        private void OpenHomeView()
        {
            CurrentView = Views[0];
            CurrentViewName = "Home";

            var vm = CurrentView as HomeViewModel;
            
            // when we click on Home, refresh the data
            vm.RefreshData();
        }
        
        [RelayCommand]
        private void OpenDashboardView()
        {
            CurrentView = Views[1];
            CurrentViewName = "Dashboard";
        }
        
        [RelayCommand]
        private void OpenFinderView()
        {
            CurrentView = Views[2];
            CurrentViewName = "Finder";
        }

        [RelayCommand]
        private void OpenObservationView()
        {
            CurrentView = Views[3];
            CurrentViewName = "Observations";
        }
        
        [RelayCommand]
        private void OpenToDoView()
        {
            CurrentView = Views[4];
            CurrentViewName = "Actions";
        }
        
        [RelayCommand]
        private void OpenEventsView()
        {
            CurrentView = Views[5];
            CurrentViewName = "Events";
        }
        
        [RelayCommand]
        private void OpenInventoryView()
        {
            CurrentView = Views[6];
            CurrentViewName = "Inventory";
        }
        
        [RelayCommand]
        private void OpenProceduresView()
        {
            CurrentView = Views[7];
            CurrentViewName = "Procedures";
        }
        
        
        [RelayCommand]
        private void OpenPlantsView()
        {
            CurrentView = Views[8];
            CurrentViewName = "Plants";
        }
        
        [RelayCommand]
        private void OpenSeedsView()
        {
            CurrentView = Views[9];
            CurrentViewName = "Starters";
        }
        
        [RelayCommand]
        private void OpenPlantingsView()
        {
            CurrentView = Views[10];
            CurrentViewName = "Plantings";
        }
        
        [RelayCommand]
        private void OpenAnimalsView()
        {
            CurrentView = Views[11];
            CurrentViewName = "Animals";
        }
        
        [RelayCommand]
        private void OpenHarvestsView()
        {
            CurrentView = Views[12];
            CurrentViewName = "Harvests";
        }
        
        [RelayCommand]
        private void OpenPreservationView()
        {
            CurrentView = Views[13];
            CurrentViewName = "Food Preservation";
        }
        
        [RelayCommand]
        private void OpenPeopleView()
        {
            CurrentView = Views[14];
            CurrentViewName = "People";
        }
        
        [RelayCommand]
        private void OpenWeatherView()
        {
            CurrentView = Views[15];
            CurrentViewName = "Weather";
        }
        
        [RelayCommand]
        private void OpenSettingsView()
        {
            CurrentView = Views[16];
            CurrentViewName = "Settings";
        }
        
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

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
            CurrentUser = AppSession.Instance.CurrentUser.FirstName;
            CurrentViewName = "Home";
            
            ScoreBoard = AppSession.Instance.CurrentScoreboard;
            
            GaiaWindow = new GaiaWindow();
        }
        
     }
}