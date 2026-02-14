using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using Ursa.Controls;

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
    
    [ObservableProperty]
    private Planting _currentPlanting;
    
    public DateTime PlantingDate { get; set; }

    [ObservableProperty] 
    private bool _showInputs;

    public bool EnablePlantingAdd = true;
    
    public WindowToastManager? ToastManager { get; set; }

    [RelayCommand]
    private void ClearPlantings()
    {
        Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode).FindAll(x => x.IsStaged));
    }
    
    [RelayCommand]
    private void SavePlantings()
    {   
        foreach (var planting in Plantings)
        {
            PlantingsService.CommitRecord(AppSession.ServiceMode, planting);
        }
        
        ToastManager?.Show(new Toast("Saved the currently staged plantings."));
    }
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
        
        CurrentPlant = Plants.FirstOrDefault();
        CurrentLocation = Beds.FirstOrDefault();
        CurrentState = States.FirstOrDefault( x => x.Code == "IP");
            
        UpdateSeedPackets();
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
        
        // get just the staged plantings
        Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode).FindAll(x => x.IsStaged));
    }

    [RelayCommand]
    private void CreatePlanting()
    {
        //check to make sure we have all the data points we need
        if (CurrentLocation != null && CurrentPlant != null && CurrentStarter != null && CurrentState != null)
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
            
            ToastManager?.Show(new Toast("Planting staged."));
        }
        else
        {
            ToastManager?.Show(new Toast("All values need to be selected before creating a planting."));
        }

    }
    
    [RelayCommand]
    private void PlantCurrentPlanting()
    {
        if (CurrentPlanting != null)
        {
            CurrentPlanting.IsPlanted = true;
            CurrentPlanting.IsStaged = false;
            PlantingsService.CommitRecord(AppSession.ServiceMode, CurrentPlanting);
            
            Console.WriteLine("Planted Planting " + CurrentPlanting.Description);

            var alert = new AlertItem()
            {
                Code = "PLANTING", Description = "Added from Staging",
                Comment = CurrentPlanting.Description + ", " + CurrentPlanting.Bed.Description
            };
            AppSession.Instance.Alerts.TryAdd(alert.Id, alert);
            
            // now get staging area data again 
            Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode).FindAll(x => x.IsStaged));
            
            ToastManager?.Show(new Toast("Staged planting now marked as planted."));
        }

    }

    [RelayCommand]
    private void RemovePlanting()
    {
        if (CurrentPlanting != null)
        {
            Console.WriteLine("Removing Planting " + CurrentPlanting.Description);
            
            // also remove from database if stored
            if (!CurrentPlanting.IsTransient)
            {
                PlantingsService.DeleteRecord(AppSession.ServiceMode, CurrentPlanting);
            }
            
            var alert = new AlertItem()
            {
                Code = "PLANTING", Description = "Removed from database",
                Comment = "ID: " + CurrentPlanting.Id + " : " + CurrentPlanting.Description + ", " + CurrentPlanting.Bed.Description
            };
            AppSession.Instance.Alerts.TryAdd(alert.Id, alert);
            
            // now get staging area data again 
            Plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode).FindAll(x => x.IsStaged));
            
            ToastManager?.Show(new Toast("Staged planting has been removed."));
        }
    }

    public PlannerViewModel()
    {
        try
        {
            ShowInputs = true;
            
            PlantingDate = DateTime.Today;
            RefreshData();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }
}