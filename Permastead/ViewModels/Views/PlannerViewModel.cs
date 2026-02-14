using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using NBitcoin.BuilderExtensions;
using Services;

namespace Permastead.ViewModels.Views;

public partial class PlannerViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<PlantingState> _states = new ObservableCollection<PlantingState>();
    
    public string PlantSelected => (CurrentPlant != null).ToString(); 
    
    [ObservableProperty]
    private Plant _currentPlant;
    
    [ObservableProperty]
    private GardenBed _currentLocation;
    
    [ObservableProperty]
    private PlantingState _currentState;
    
    [ObservableProperty]
    private SeedPacket _currentStarter;
    
    public DateTime PlantingDate { get; set; }

    [ObservableProperty] 
    private bool _showInputs;
    
    [RelayCommand]
    private void ClearPlantings()
    {
        Plantings.Clear();
    }
    
    [RelayCommand]
    private void SavePlantings()
    {   
        foreach (var planting in Plantings)
        {
            PlantingsService.CommitRecord(AppSession.ServiceMode, planting);
        }
    }
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }
    
    [RelayCommand]
    private void UpdateSeedPackets()
    {
        SeedPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPacketForPlant(AppSession.ServiceMode, CurrentPlant.Id).OrderBy(x => x.StartDate));
    }

    public void RefreshDataOnly()
    {
        Beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        SeedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, false).FindAll(x => x.EndDate > DateTime.Now));
        Plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        States = new ObservableCollection<PlantingState>(PlantingsService.GetPlantingStates(AppSession.ServiceMode));
    }

    [RelayCommand]
    private void CreatePlanting()
    {
        var newPlanting = new Planting();

        newPlanting.Plant = CurrentPlant;
        newPlanting.IsPlanted = false;
        newPlanting.IsStaged = true;
        newPlanting.Bed = CurrentLocation;
        newPlanting.SeedPacket = CurrentStarter;
        newPlanting.State = CurrentState;
        newPlanting.Author = AppSession.Instance.CurrentUser;
        newPlanting.Description = CurrentPlant.Description;
        newPlanting.Code = CurrentStarter.Code;
        newPlanting.StartDate = PlantingDate;
        
        Plantings.Add(newPlanting);

    }

    public PlannerViewModel()
    {
        try
        {
            ShowInputs = true;
            
            PlantingDate = DateTime.Today;
            RefreshData();
            CurrentPlant = Plants.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }
}