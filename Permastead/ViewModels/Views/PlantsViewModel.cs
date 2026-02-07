using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Models;
using Services;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Permastead.Views.Views;

using Serilog;
using Serilog.Context;

using Ursa.Controls;

namespace Permastead.ViewModels.Views;

public partial class PlantsViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<TagData> _items = new ObservableCollection<TagData>();
    
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

    [ObservableProperty] private long _plantingsCount;

    [ObservableProperty] private long _seedsCount;

    [ObservableProperty] private double _successRate;

    [ObservableProperty] private string _currentObservations;
    
    [ObservableProperty] private string _searchText = "";
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this plant?";
    private string _message;
    private string? _title = "Deletion Confirmation";
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }
    
    public ICommand YesNoCommand { get; set; }
    
    public WindowToastManager? ToastManager { get; set; }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }

    [RelayCommand]
    private async void DeletePlant()
    {
        bool rtnValue = false;

        try
        {
            if (CurrentPlant != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    rtnValue = Services.PlantService.DeleteRecord(AppSession.ServiceMode, CurrentPlant);
                    ToastManager?.Show(new Toast("Plant record (" + CurrentPlant.Description + ") has been removed."));

                    OnPropertyChanged(nameof(CurrentPlant));

                    using (LogContext.PushProperty("PlantsViewModel", this))
                    {
                        if (CurrentPlant.Id == 0) CurrentPlant.Author = AppSession.Instance.CurrentUser;
                        Log.Information("Removed plant: " + CurrentPlant.Description, rtnValue);

                    }

                    RefreshDataOnly();
                }
            }
        }
        catch (Exception e)
        {
        }
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
        try
        {
            var win = new PlantingWizard();
            var vm = new PlantingWizardViewModel();
            vm.ControlViewModel = new PlantingsViewModel();
            
            // set up some intelligent defaults
            vm.ControlViewModel.CurrentItem.Description = CurrentPlant.Description;
            vm.CurrentPlant = CurrentPlant;
            vm.CurrentPlanting.Code = CurrentPlant.Code;
            
            win.DataContext = vm;
            
            win.Title = "New Planting: " + CurrentPlant!.Description;
            win.Topmost = true;
            win.Width = 800;
            win.Height = 550;
            win.Opacity = 0.95;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            win.Show();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
        
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.Question;
        _message = _shortMessage;
        
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

        SeedsCount = SeedPackets.Count;
        PlantingsCount = Plantings.Count;
        
        // calc the success rate
        long successCount = 0;
        foreach (var planting in Plantings)
        {
            if (planting.State.Code != "DEAD") successCount++;   
        }

        if (PlantingsCount > 0)
            SuccessRate = 100.0 * ((double)successCount / (double)PlantingsCount);
        else
            SuccessRate = 0.0;

        if (SelectedItems != null)
        {
            SelectedItems.Clear();
            foreach (var tag in CurrentPlant.TagList)
            {
                SelectedItems.Add( new TagData { TagText = tag } );
            }
        }
        
        
        var sb = new StringBuilder();
        var obs = new List<PlantingObservation>();
        
        foreach (var pt in Plantings)
        {
            obs.Add(new PlantingObservation() { AsOfDate = pt.StartDate, Planting = pt, Comment = " -> PLANTING: " + pt.SeedPacket.Vendor.Description + " : " + pt.Comment });
            obs.AddRange(PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, pt.Id));
        }

        obs = obs.OrderByDescending(x=> x.AsOfDate).ToList();
        
        foreach (var o in obs)
        {
            sb.AppendLine(o.AsOfDate.ToShortDateString() + ": (" + o.Planting.Bed.Code + ") " + o.Planting.Bed.Description + " | " +
                          o.Planting.Description);
            sb.AppendLine(o.Comment);
            sb.AppendLine();
        }
        
        CurrentObservations = sb.ToString();

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
                    p.Tags.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
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
        
        Items.Clear();
        Items = new ObservableCollection<TagData>(Services.PlantService.GetAllTags(AppSession.ServiceMode));
        
    }
    
        
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task Show(MessageBoxButton button)
    {
        Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
    }
    
}