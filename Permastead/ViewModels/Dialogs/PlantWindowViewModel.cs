using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;
using Ursa.Controls;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantWindowViewModel : ViewModelBase
{
    public ObservableCollection<TagData> Items { get; set; }
    public ObservableCollection<TagData> SelectedItems { get; set; }
    public AutoCompleteFilterPredicate<object> FilterPredicate { get; set; }
    
    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] 
    private Plant _plant;
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();

    public WindowToastManager? ToastManager { get; set; }
    
    private PlantingsViewModel _controlViewModel { get; set;  } = new PlantingsViewModel();
    
    public PlantWindowViewModel()
    {
        _plant = new Plant();
    }

    public PlantWindowViewModel(Plant plant, PlantingsViewModel obsVm) : this()
    {
        _plant = plant;
        _controlViewModel = obsVm;
        
        Items = new ObservableCollection<TagData>(Services.PlantService.GetAllTags(AppSession.ServiceMode));
        SelectedItems = new ObservableCollection<TagData>();
        FilterPredicate = Search;
        
        if (Plant != null)
        {
            foreach (var tagData in Plant.TagList)
            {
                var td = new TagData
                {
                    TagText = tagData
                };
                SelectedItems.Add(td);
            }
        }

        RefreshData();
    }
    
    private static bool Search(string? text, object? data)
    {
        if (text is null) return true;
        
        if (data is not TagData control) return false;
        return control.TagText.Contains(text, StringComparison.InvariantCultureIgnoreCase);
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
        
        _controlViewModel.ToastManager.Show(new Toast("Plant record (" + _plant.Description + ") has been updated."));
        
        _controlViewModel.RefreshData();
        
    }

    public void RefreshData()
    {
        _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPacketForPlant(AppSession.ServiceMode, Plant.Id));
        _plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantingsForPlant(AppSession.ServiceMode, Plant.Id));
        
        _controlViewModel.RefreshData();
    }
    
}