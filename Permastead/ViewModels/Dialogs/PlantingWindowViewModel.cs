using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantingWindowViewModel : ViewModelBase
{
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
    
    private PlantingsViewModel _controlViewModel { get; set;  } = new PlantingsViewModel();
    
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
        _controlViewModel = obsVm;
        
        if (Planting!.Plant.Id != 0 && _plants.Count > 0) Planting.Plant = Plants.First(x => x.Id == Planting.Plant.Id);
        if (Planting.SeedPacket.Id != 0 && SeedPackets.Count > 0) Planting.SeedPacket = SeedPackets.First(x => x.Id == Planting.SeedPacket.Id);
        if (Planting.Author.Id != 0 && People.Count > 0) Planting.Author = People.First(x => x.Id == Planting.Author.Id);
        if (Planting.State.Id != 0 && PlantingStates.Count > 0) Planting.State = PlantingStates.First(x => x.Id == Planting.State.Id);
    }

    public void SavePlanting()
    {
        bool rtnValue;
        
        rtnValue = Services.PlantingsService.CommitRecord(AppSession.ServiceMode, _planting);
        
        OnPropertyChanged(nameof(_planting));
            
        using (LogContext.PushProperty("PlantingViewModel", this))
        {
            Log.Information("Saved planting: " + _planting.Description, rtnValue);
        }
        
        _controlViewModel.RefreshData();
        
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
        
        _controlViewModel.RefreshData();
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
        
        _controlViewModel.RefreshData();
        
    }
}