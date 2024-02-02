using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Permastead.ViewModels.Views;

namespace Permastead.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _currentUser;
        
        private ViewModelBase[] Views =
        {
            new HomeViewModel(),
            new DashboardViewModel(),
            new ObservationsViewModel(),
            new ToDoViewModel(),
            new EventsViewModel(),
            new InventoryViewModel(),
            new SeedsViewModel(),
            new PlantingsViewModel(),
            new ContactsViewModel(),
            new SettingsViewModel()
        };
        

        [ObservableProperty] 
        private ViewModelBase _CurrentView;
  
    
        [ObservableProperty] 
        private string _CurrentViewName;

        [RelayCommand]
        private void OpenHomeView()
        {
            CurrentView = Views[0];
            CurrentViewName = "Home";
        }
        
        [RelayCommand]
        private void OpenDashboardView()
        {
            CurrentView = Views[1];
            CurrentViewName = "Dashboard";
        }

        [RelayCommand]
        private void OpenObservationView()
        {
            CurrentView = Views[2];
            CurrentViewName = "Observations";
        }
        
        [RelayCommand]
        private void OpenToDoView()
        {
            CurrentView = Views[3];
            CurrentViewName = "Actions";
        }
        
        [RelayCommand]
        private void OpenEventsView()
        {
            CurrentView = Views[4];
            CurrentViewName = "Events";
        }
        
        [RelayCommand]
        private void OpenInventoryView()
        {
            CurrentView = Views[5];
            CurrentViewName = "Inventory";
        }
        
        [RelayCommand]
        private void OpenSeedsView()
        {
            CurrentView = Views[6];
            CurrentViewName = "Seeds";
        }
        
        [RelayCommand]
        private void OpenPlantingsView()
        {
            CurrentView = Views[7];
            CurrentViewName = "Plantings";
        }
        
        [RelayCommand]
        private void OpenPeopleView()
        {
            CurrentView = Views[8];
            CurrentViewName = "People";
        }
        
        [RelayCommand]
        private void OpenSettingsView()
        {
            CurrentView = Views[9];
            CurrentViewName = "Settings";
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
            CurrentUser = AppSession.Instance.CurrentUser.FirstName;
        }
        
     }
}