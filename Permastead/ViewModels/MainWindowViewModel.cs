using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Permastead.ViewModels.Views;

namespace Permastead.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
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
        }
        
        [RelayCommand]
        private void OpenDashboardView()
        {
            CurrentView = Views[1];
        }

        [RelayCommand]
        private void OpenObservationView()
        {
            CurrentView = Views[2];
        }
        
        [RelayCommand]
        private void OpenToDoView()
        {
            CurrentView = Views[3];
        }
        
        [RelayCommand]
        private void OpenEventsView()
        {
            CurrentView = Views[4];
        }
        
        [RelayCommand]
        private void OpenInventoryView()
        {
            CurrentView = Views[5];
        }
        
        [RelayCommand]
        private void OpenSeedsView()
        {
            CurrentView = Views[6];
        }
        
        [RelayCommand]
        private void OpenPlantingsView()
        {
            CurrentView = Views[7];
        }
        
        [RelayCommand]
        private void OpenPeopleView()
        {
            CurrentView = Views[8];
        }
        
        [RelayCommand]
        private void OpenSettingsView()
        {
            CurrentView = Views[9];
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
        }
        
     }
}