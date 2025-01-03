using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.Views.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Views;

public partial class PlantsViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty] 
    private Plant _currentPlant;
    
    [ObservableProperty] 
    private long _plantsCount;
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }

    public PlantsViewModel()
    {
        RefreshDataOnly();
    }

    public void GetMetaData()
    {
        SeedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPacketForPlant(AppSession.ServiceMode, CurrentPlant.Id));
        Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantingsForPlant(AppSession.ServiceMode, CurrentPlant.Id));
        
    }
    
    public void RefreshDataOnly()
    {
        var myPlants = Services.PlantService.GetAllPlants(AppSession.ServiceMode);

        _plants.Clear();
        foreach (var p in myPlants)
        {
            _plants.Add(p);
            if (CurrentPlant == null) CurrentPlant = p;
        }
        
        PlantsCount = _plants.Count;
        if (CurrentPlant != null) GetMetaData();

    }

    public void SavePlant()
    {
        bool rtnValue;

        rtnValue = Services.PlantService.CommitRecord(AppSession.ServiceMode, CurrentPlant);
        
        OnPropertyChanged(nameof(CurrentPlant));
            
        using (LogContext.PushProperty("PlantsViewModel", this))
        {
            if (CurrentPlant.Id == 0) CurrentPlant.Author = AppSession.Instance.CurrentUser;
            Log.Information("Saved planting: " + CurrentPlant.Description, rtnValue);
        }
        
        RefreshDataOnly();
        
    }
    
}