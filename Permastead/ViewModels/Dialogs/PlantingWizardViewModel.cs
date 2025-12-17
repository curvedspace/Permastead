using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Models;

using Permastead.ViewModels.Views;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantingWizardViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Plant? _currentPlant;
    
    [ObservableProperty] 
    private Planting? _currentPlanting;
    
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<PlantingState> _plantingStates = new ObservableCollection<PlantingState>();
    
    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors = new ObservableCollection<Vendor>();
    
    [ObservableProperty]
    private ObservableCollection<Seasonality> _seasonalities;
    
    [ObservableProperty]
    private ObservableCollection<StarterType> _starterTypes;
    
    public PlantingsViewModel ControlViewModel { get; set;  } = new PlantingsViewModel();
    
    public PlantingWizardViewModel() 
    {
        CurrentPlanting = new Planting(); 
        CurrentPlanting.StartDate = DateTime.Today;
        
        Beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        PlantingStates = new ObservableCollection<PlantingState>(Services.PlantingsService.GetPlantingStates(AppSession.ServiceMode));
        Plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        Vendors = new ObservableCollection<Vendor>(Services.VendorService.GetAll(AppSession.ServiceMode));
        Seasonalities = new ObservableCollection<Seasonality>(Services.PlantingsService.GetSeasonalities(AppSession.ServiceMode));
        StarterTypes = new ObservableCollection<StarterType>(Services.PlantingsService.GetStarterTypes(AppSession.ServiceMode));
    }
}