﻿using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
        
        [ObservableProperty] private ScoreBoard _scoreBoard;
        
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
            new PlantsViewModel(),
            new SeedsViewModel(),
            new PlantingsViewModel(),
            new HarvestsViewModel(),
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
        private void OpenPlantsView()
        {
            CurrentView = Views[6];
            CurrentViewName = "Plants";
        }
        
        [RelayCommand]
        private void OpenSeedsView()
        {
            CurrentView = Views[7];
            CurrentViewName = "Seeds";
        }
        
        [RelayCommand]
        private void OpenPlantingsView()
        {
            CurrentView = Views[8];
            CurrentViewName = "Plantings";
        }
        
        [RelayCommand]
        private void OpenHarvestsView()
        {
            CurrentView = Views[9];
            CurrentViewName = "Harvests";
        }
        
        [RelayCommand]
        private void OpenPeopleView()
        {
            CurrentView = Views[10];
            CurrentViewName = "People";
        }
        
        [RelayCommand]
        private void OpenWeatherView()
        {
            CurrentView = Views[11];
            CurrentViewName = "Weather";
        }
        
        [RelayCommand]
        private void OpenSettingsView()
        {
            CurrentView = Views[12];
            CurrentViewName = "Settings";
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
            CurrentUser = AppSession.Instance.CurrentUser.FirstName;
            CurrentViewName = "Home";
            
            ScoreBoard = AppSession.Instance.CurrentScoreboard;
        }
        
     }
}