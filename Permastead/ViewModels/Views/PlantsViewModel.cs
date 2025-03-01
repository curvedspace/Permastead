using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
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

    [RelayCommand]
    private void DeletePlant()
    {
        bool rtnValue;

        rtnValue = Services.PlantService.DeleteRecord(AppSession.ServiceMode, CurrentPlant);
        
        OnPropertyChanged(nameof(CurrentPlant));
            
        using (LogContext.PushProperty("PlantsViewModel", this))
        {
            if (CurrentPlant.Id == 0) CurrentPlant.Author = AppSession.Instance.CurrentUser;
            Log.Information("Removed plant: " + CurrentPlant.Description, rtnValue);
        }
        
        RefreshDataOnly();
    }
    
    [RelayCommand]
    private void AddStarter()
    {
        var win = new StarterWindow();
        
        var newSeedPacket = new SeedPacket();
        newSeedPacket.Author = AppSession.Instance.CurrentUser;
        newSeedPacket.Description = CurrentPlant.Description;
        newSeedPacket.Plant!.Id = CurrentPlant.Id;
        newSeedPacket.Code = CurrentPlant.Code;

        
        var seedsVm = new SeedsViewModel();
        var vm = new StarterWindowViewModel(newSeedPacket);
        
        
        vm.ControlViewModel =  new SeedsViewModel();
        
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 800;
        win.Height = 650;
        win.Opacity = 0.95;
        win.Title = "New Plant Starter";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
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