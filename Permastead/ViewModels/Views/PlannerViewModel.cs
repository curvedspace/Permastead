using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class PlannerViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty]
    private Plant _currentPlant;
    
    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<PlantingState> _states = new ObservableCollection<PlantingState>();
    

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

    public PlannerViewModel()
    {
        try
        {
            RefreshData();
            CurrentPlant = Plants.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }
}