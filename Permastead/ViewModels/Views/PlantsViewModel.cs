using System;
using System.Collections.ObjectModel;
//using System.Drawing;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Permastead.Views.Views;
using Serilog;
using Serilog.Context;
using Services;

namespace Permastead.ViewModels.Views;

public partial class PlantsViewModel : ViewModelBase
{
    
    public ObservableCollection<TagData> Items { get; set; }
    public ObservableCollection<TagData> SelectedItems { get; set; }
    public AutoCompleteFilterPredicate<object> FilterPredicate { get; set; }
    
    [ObservableProperty] 
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty]
    private ObservableCollection<string> _tags = new ObservableCollection<string>();
    
    [ObservableProperty]
    private Bitmap picture; 
    
    [ObservableProperty] 
    private Plant _currentPlant;
    
    [ObservableProperty] 
    private long _plantsCount;
    
    [ObservableProperty] private string _searchText = "";
    

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
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
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
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

    [RelayCommand]
    private void AddPlanting()
    {
        
    }


    public PlantsViewModel()
    {
        RefreshDataOnly();
        AssetLoader.GetAssets(new Uri("avares://Permastead"),null);
        
        Picture = new Bitmap(AssetLoader.Open(new Uri("avares://Permastead/Assets/plant_icon.png"), null));

        Items = new ObservableCollection<TagData>(Services.PlantService.GetAllTags(AppSession.ServiceMode));
        SelectedItems = new ObservableCollection<TagData>();

        if (CurrentPlant != null)
        {
            foreach (var tagData in CurrentPlant.TagList)
            {
                var td = new TagData
                {
                    TagText = tagData
                };
                SelectedItems.Add(td);
            }
        }
        
        
        FilterPredicate = Search;
    }
    
    private static bool Search(string? text, object? data)
    {
        if (text is null) return true;
        
        if (data is not TagData control) return false;
        return control.TagText.Contains(text, StringComparison.InvariantCultureIgnoreCase);
    }

    public void GetMetaData()
    {
        SeedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPacketForPlant(AppSession.ServiceMode, CurrentPlant.Id));
        Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantingsForPlant(AppSession.ServiceMode, CurrentPlant.Id));
        
        //look up image, if available, for this plant
        Picture = PlantService.GetPlantImage(CurrentPlant);
        
        Tags.Clear();
        Tags.Add("herb");
        Tags.Add("bitter");

        if (SelectedItems != null)
        {
            SelectedItems.Clear();
            foreach (var tag in CurrentPlant.TagList)
            {
                SelectedItems.Add( new TagData { TagText = tag } );
            }
        }

    }
    
    public void RefreshDataOnly(string filterText = "")
    {
        var myPlants = Services.PlantService.GetAllPlants(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();

        Plants.Clear();
        
        foreach (var p in myPlants)
        {
            
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                _plants.Add(p);
                if (CurrentPlant == null) CurrentPlant = p;
            }
            else
            {
                if (p.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    p.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    p.Family.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    _plants.Add(p);
                }
            }
            
        }
        
        PlantsCount = _plants.Count;
        if (CurrentPlant != null) GetMetaData();

    }

    public void SavePlant()
    {
        bool rtnValue;

        //write out tags into text string for storage
        if (CurrentPlant != null)
        {
            CurrentPlant.TagList.Clear();

            foreach (var tagData in SelectedItems)
            {
                CurrentPlant.TagList.Add(tagData.TagText);
            }
            
            CurrentPlant.SyncTags();
            rtnValue = Services.PlantService.CommitRecord(AppSession.ServiceMode, CurrentPlant);
            
            OnPropertyChanged(nameof(CurrentPlant));
                
            using (LogContext.PushProperty("PlantsViewModel", this))
            {
                if (CurrentPlant.Id == 0) CurrentPlant.Author = AppSession.Instance.CurrentUser;
                Log.Information("Saved planting: " + CurrentPlant.Description, rtnValue);
            }
        }
        
        RefreshDataOnly();
        
    }
    
}