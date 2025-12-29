using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;
using Ursa.Controls;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantingWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _allowClear = true;
    [ObservableProperty] private bool _allowHalf = true;
    [ObservableProperty] private bool _isEnabled = true;

    [ObservableProperty] private double _defaultValue = 0;
    [ObservableProperty] private int _count = 5;
    
    [ObservableProperty]
    private Planting? _planting;
 
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();

    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty]
    private ObservableCollection<PlantingState> _plantingStates = new ObservableCollection<PlantingState>();

    [ObservableProperty]
    private ObservableCollection<PlantingObservation> _plantingObservations = new ObservableCollection<PlantingObservation>();
    
    public PlantingsViewModel ControlViewModel { get; set;  } = new PlantingsViewModel();
    
    public PlantingWindowViewModel() 
    {
        Plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        SeedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode));
        Beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        People = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        PlantingStates = new ObservableCollection<PlantingState>(Services.PlantingsService.GetPlantingStates(AppSession.ServiceMode));
    }
    
    public PlantingWindowViewModel(Planting planting, PlantingsViewModel obsVm) : this()
    {
        _planting = planting;
        ControlViewModel = obsVm;

        Planting.YieldRatingValue = Convert.ToDouble(Planting.YieldRating / 20m); //divide by 20 to get the yield in a 0-5 range
        
        if (Planting!.Plant.Id != 0 && _plants.Count > 0) Planting.Plant = Plants.First(x => x.Id == Planting.Plant.Id);
        if (Planting.SeedPacket.Id != 0 && SeedPackets.Count > 0) Planting.SeedPacket = SeedPackets.First(x => x.Id == Planting.SeedPacket.Id);
        if (Planting.Author.Id != 0 && People.Count > 0) Planting.Author = People.First(x => x.Id == Planting.Author.Id);
        if (Planting.State.Id != 0 && PlantingStates.Count > 0) Planting.State = PlantingStates.First(x => x.Id == Planting.State.Id);
        
        OnPropertyChanged(nameof(Planting));
    }

    
    public void SavePlanting()
    {
        bool rtnValue;

        Planting.YieldRating = (decimal)Planting.YieldRatingValue * 20;
        rtnValue = Services.PlantingsService.CommitRecord(AppSession.ServiceMode, _planting);
        
        OnPropertyChanged(nameof(_planting));
            
        using (LogContext.PushProperty("PlantingViewModel", this))
        {
            Log.Information("Saved planting: " + _planting.Description, rtnValue);
        }
        
        ControlViewModel.RefreshData();
        ControlViewModel.ToastManager?.Show(new Toast("Planting has been updated."));
        
    }
    
    public void HarevstPlanting()
    {
        bool rtnValue;
        Planting!.EndDate = DateTime.Today;
        Planting.State = PlantingStates.First(x => x.Code == "H");
        rtnValue = Services.PlantingsService.CommitRecord(AppSession.ServiceMode, Planting);
        
        OnPropertyChanged(nameof(Planting));
            
        using (LogContext.PushProperty("PlantingViewModel", this))
        {
            Log.Information("Harevsted planting: " + Planting.Description, rtnValue);
        }
        
        ControlViewModel.RefreshData();
    }
    
    public void TerminatePlanting()
    {
        bool rtnValue;
        Planting!.EndDate = DateTime.Today;
        Planting.State = PlantingStates.First(x => x.Code == "DEAD");
        rtnValue = Services.PlantingsService.CommitRecord(AppSession.ServiceMode, Planting);
        
        OnPropertyChanged(nameof(Planting));
            
        using (LogContext.PushProperty("PlantingViewModel", this))
        {
            Log.Information("Terminated planting: " + Planting.Description, rtnValue);
        }
        
        ControlViewModel.RefreshData();
        ControlViewModel.ToastManager?.Show(new Toast("Planting has been terminated."));
        
    }
}