using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantWindowViewModel : ViewModelBase
{
    
    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] 
    private Plant _plant;
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();


    private PlantingsViewModel _controlViewModel { get; set;  } = new PlantingsViewModel();
    
    public PlantWindowViewModel()
    {
        _plant = new Plant();
    }

    public PlantWindowViewModel(Plant plant, PlantingsViewModel obsVm) : this()
    {
        _plant = plant;
        _controlViewModel = obsVm;
        
        _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPacketForPlant(AppSession.ServiceMode, Plant.Id));
        _plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantingsForPlant(AppSession.ServiceMode, Plant.Id));
    }

    public void SavePlant()
    {
        bool rtnValue;

        rtnValue = Services.PlantService.CommitRecord(AppSession.ServiceMode, _plant);
        
        OnPropertyChanged(nameof(_plant));
            
        using (LogContext.PushProperty("PlantViewModel", this))
        {
            if (_plant.Id == 0) _plant.Author = AppSession.Instance.CurrentUser;
            
            Log.Information("Saved planting: " + _plant.Description, rtnValue);
        }
        
        _controlViewModel.RefreshData();
        
    }
    
}