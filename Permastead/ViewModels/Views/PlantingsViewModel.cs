using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using System.Reactive.PlatformServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;
using Nostr.Client.Identifiers;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Serilog;
using Services;
using Ursa.Controls;

namespace Permastead.ViewModels.Views;

public partial class PlantingsViewModel : ViewModelBase
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
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty] 
    private ObservableCollection<Vendor> _vendors = new ObservableCollection<Vendor>();

    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] private Node _SelectedNode;
    
    [ObservableProperty] 
    private bool _currentOnly = true;   //show only current plantings by default
    
    [ObservableProperty] 
    private Planting _currentItem;
    
    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty] private string _searchText = "";
    
    [ObservableProperty] private PlantingObservation _currentObservation = new PlantingObservation();
    
    [ObservableProperty]
    private ObservableCollection<PlantingObservation> _plantingObservations = new ObservableCollection<PlantingObservation>();
    
    [ObservableProperty] private ObservableCollection<Node> _nodes;

    [ObservableProperty] private ObservableCollection<Node> _selectedNodes;
    
    public FlatTreeDataGridSource<Planting> PlantingsSource { get; set; }
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this planting?";
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
    
    public PlantingsViewModel()
    {
        try
        {
            
            _currentItem = new Planting();
            
            PlantingObservations =
                new ObservableCollection<PlantingObservation>(
                    Services.PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, CurrentItem.Id));

            RefreshPlantings();
            
            YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
            Icons = new ObservableCollection<MessageBoxIcon>(
                Enum.GetValues<MessageBoxIcon>());
            SelectedIcon = MessageBoxIcon.Question;
            _message = _shortMessage;
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting plantings.");
        }
            
    }

    [RelayCommand]
    private void RefreshPlantings()
    {
        _beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, false));
        _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        _vendors = new ObservableCollection<Vendor>(VendorService.GetAll(AppSession.ServiceMode));

        
        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>();

        SelectedNode = null;
        
        // populate treeview with plants
        var plantsNode = new Node("Plants (" + _plants.Count + ")", new ObservableCollection<Node>());
        Nodes.Add(plantsNode);
        foreach (var p in _plants)
        {
            plantsNode.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Plant));
        }
        
        // now location
        var locNode = new Node("Locations (" + _beds.Count + ")", new ObservableCollection<Node>());
        Nodes.Add(locNode);
        foreach (var b in _beds)
        {
            locNode.SubNodes.Add(new Node(b.Id, b.Code + ": " + b.Description, NodeType.GardenBed));
        }
        
        // now vendors
        var vendorNode = new Node("Vendors (" + _vendors.Count + ")", new ObservableCollection<Node>());
        Nodes.Add(vendorNode);
        foreach (var b in _vendors)
        {
            vendorNode.SubNodes.Add(new Node(b.Id, b.Description, NodeType.Vendor));
        }
        
        this.RefreshData();
    }

    [RelayCommand]
    private void RefreshDataOnly(string filterText = "")
    {
        RefreshData(null,filterText);
    }
    
    public void RefreshData(Node node = null, string filterText = "")
    {
        // clear out plantings, prepare for filtering
        Plantings.Clear();

        if (filterText == null) filterText = "";
        if (node != null) SelectedNode = node;
        
        var p1 = Services.PlantingsService.GetPlantingsByPlantedDate(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();

        bool addRecord = false;
        foreach (var o in p1)
        {
            addRecord = false;

            if (SelectedNode != null)
            {
                switch (SelectedNode.Type)
                {
                    case NodeType.Plant:
                        if (o.Plant.Description == SelectedNode.Title)
                        {
                            if (!string.IsNullOrEmpty(caseAdjustedFilterText))
                            {
                                if (o.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Plant.Description.ToLowerInvariant().Contains(caseAdjustedFilterText))
                                {
                                    addRecord = true;
                                }
                                else
                                {
                                    addRecord = false;
                                }
                            }
                            else
                            {
                                addRecord = true;
                            }
                        }

                        break;
                    case NodeType.GardenBed:
                        if (o.Bed.Code + ": " + o.Bed.Description == SelectedNode.Title)
                        {
                            if (!string.IsNullOrEmpty(caseAdjustedFilterText))
                            {
                                if (o.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Plant.Description.ToLowerInvariant().Contains(caseAdjustedFilterText))
                                {
                                    addRecord = true;
                                }
                                else
                                {
                                    addRecord = false;
                                }
                            }
                            else
                            {
                                addRecord = true;
                            }
                        }
                        break;
                    case NodeType.Vendor:
                        if (o.SeedPacket.Vendor.Description == SelectedNode.Title) 
                        {
                            if (!string.IsNullOrEmpty(caseAdjustedFilterText))
                            {
                                if (o.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                                    o.Plant.Description.ToLowerInvariant().Contains(caseAdjustedFilterText))
                                {
                                    addRecord = true;
                                }
                                else
                                {
                                    addRecord = false;
                                }
                            }
                            else
                            {
                                addRecord = true;
                            }
                        }
                        break;
                    default:
                        addRecord = false;
                        break;
                }
            }
            else
            {
                if (o.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    o.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    o.Plant.Description.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    addRecord = true;
                }
                else
                {
                    addRecord = false;
                }
            }
            
            if (CurrentOnly)
            {
                if (o.IsActive && addRecord) Plantings.Add(o);
            }
            else
            {
                if (addRecord) Plantings.Add(o);
            }
        }

        if (Plantings.Count > 0) 
            CurrentItem = Plantings.FirstOrDefault()!;
        else
            CurrentItem = new Planting();

        PlantingCount = Plantings.Count;
        
        PlantingsSource = new FlatTreeDataGridSource<Planting>(Plantings)
        {
            Columns =
            {
                new TextColumn<Planting, string>
                    ("Date", x => x.StartDateString),
                new TextColumn<Planting, string>
                    ("Description", x => x.Description),
                new TextColumn<Planting, string>
                    ("State", x => x.State.Description),
                new TextColumn<Planting, string>
                    ("Author", x => x.Author.FirstName),
                new TextColumn<Planting, string>
                    ("Type", x => x.Plant.Description),
                new TextColumn<Planting, decimal>
                    ("Yield", x => x.YieldRating),
                new TextColumn<Planting, string>
                    ("Age", x => x.Age),
                new TextColumn<Planting, decimal>
                    ("DTM", x => x.SeedPacket.DaysToHarvest),
                new TextColumn<Planting, string>
                    ("Location", x => x.Bed.Code),
                new TextColumn<Planting, string>
                    ("Location Description", x => x.Bed.Description),
                new TextColumn<Planting, string>
                    ("Starter", x => x.SeedPacket.Description),
                new TextColumn<Planting, string>
                    ("Comment", x => x.Comment)
            },
        };

    }
    
    [RelayCommand]
    private void SaveEvent()
    {
        // Saves a planting record
        bool rtnValue;
        
        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {

            CurrentItem.CreationDate = DateTime.Now;

            rtnValue = PlantingsService.CommitRecord(AppSession.ServiceMode, CurrentItem);
            
            if (rtnValue)
            {
                Plantings.Add(CurrentItem);
            }
            
            Console.WriteLine("saved " + rtnValue);

            RefreshPlantings();
        }
        else
        {
            rtnValue = PlantingsService.CommitRecord(AppSession.ServiceMode, CurrentItem);
            RefreshPlantings();
        }

    }
    
    public void GetPlantingObservations()
    {
        if (CurrentItem != null)
        {
            PlantingObservations =
                new ObservableCollection<PlantingObservation>(
                    Services.PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, CurrentItem.Id));
        }
    }

    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author = AppSession.Instance.CurrentUser;
            CurrentObservation.Planting = CurrentItem;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 7;
            
            PlantingsService.AddPlantingObservation(AppSession.ServiceMode, CurrentObservation);
            
            PlantingObservations =
                new ObservableCollection<PlantingObservation>(
                    Services.PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, CurrentItem.Id));

            CurrentObservation = new PlantingObservation();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    [RelayCommand]
    private void ResetEvent()
    {
        RefreshPlantings();
        
        // reset the current item
        CurrentItem = new Planting();
        OnPropertyChanged(nameof(CurrentItem));
        
    }
    
    [RelayCommand]
    private void EditPlanting()
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new PlantingWindow();
        
        //get the selected row in the list
        var current = CurrentItem;
        if (current != null)
        {
            var vm = new PlantingWindowViewModel(current, this);

            plantingWindow.DataContext = vm;

            plantingWindow.Topmost = true;
            plantingWindow.Width = 875;
            plantingWindow.Height = 600;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Planting - " + current.Description;
            plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            plantingWindow.Show();
        }
    }

    [RelayCommand]
    private async void RemovePlanting()
    {
        try
        {
            if (CurrentItem != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    //remove the record
                    PlantingsService.DeleteRecord(AppSession.ServiceMode, CurrentItem);
                    RefreshDataOnly(SearchText);
                    ToastManager?.Show(new Toast("Planting record has been removed."));
                }
            }
        }
        catch (Exception e)
        {
        }
    }
    
    [RelayCommand]
    private void CreateHarvest()
    {
        try
        {
            // open the selected planting in a harvest window 
            var harvestWindow = new HarvestWindow();
        
            //get the selected row in the list
            var current = CurrentItem;
        
            if (current != null)
            {
                var harvest = new Harvest();
                harvest.Author = AppSession.Instance.CurrentUser;
                harvest.Description = current.Description;
            
                var harvestTypes = HarvestService.GetAllHarvestTypes(AppSession.ServiceMode);
                var plantType = harvestTypes.FirstOrDefault(x => x.Description.ToLowerInvariant() == "plant");

                harvest.HarvestType = plantType;
                harvest.HarvestEntity.Id = current.Id;

                var hvm = new HarvestsViewModel();
                hvm.CurrentItem = harvest;
            
            
                var vm = new HarvestWindowViewModel(harvest, hvm);

                harvestWindow.DataContext = vm;

                harvestWindow.Topmost = true;
                harvestWindow.Width = 1000;
                harvestWindow.Height = 600;
                harvestWindow.Opacity = 0.95;
                harvestWindow.Title = "Harvest - " + current.Description;
                harvestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                harvestWindow.Show();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [RelayCommand]
    private void SaveSeeds()
    {
        var win = new StarterWindow();
        
        if (CurrentItem == null) return;

        var sp = PlantingsService.GetSeedPacketFromId(AppSession.ServiceMode, CurrentItem.SeedPacketId);
        var vm = new StarterWindowViewModel(sp);
        
        vm.SeedPacket.CreationDate = DateTime.Now;
        vm.SeedPacket.StartDate = DateTime.UtcNow;
        vm.SeedPacket.EndDate = vm.SeedPacket.StartDate.AddYears(5);
        vm.SeedPacket.Generations += 1;
        vm.SeedPacket.Id = 0;   //make it a new record
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 800;
        win.Height = 650;
        win.Opacity = 0.95;
        win.Title = "Save Seeds from " + CurrentItem.Description;
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    [RelayCommand]
    private void PropagatePlanting()
    {
        //take the current planting and make a new record from it
        var plantingWindow = new PlantingWindow();
        
        //get the selected row in the list
        var current = CurrentItem.Clone(CurrentItem);
        if (current != null)
        {
            current.Id = 0;
            current.StartDate = DateTime.UtcNow;
            current.YieldRating = 0;
            current.Author = AppSession.Instance.CurrentUser;
            current.Comment = "Propagated from mother planting ID: " + CurrentItem.Id + " (" + CurrentItem.SeedPacket.Description + ", " + CurrentItem.StartDate.Year + ")";
            
            var vm = new PlantingWindowViewModel(current, this);

            plantingWindow.DataContext = vm;

            plantingWindow.Topmost = true;
            plantingWindow.Width = 1000;
            plantingWindow.Height = 600;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Propagation of " + current.Description;
            plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            plantingWindow.Show();
        }
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
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

public class Node
{
    public ObservableCollection<Node>? SubNodes { get; }
    public long Id { get; }
    public string Title { get; set; }
    
    public NodeType Type { get; }

    public Node(long id,string title, NodeType type)
    {
        Id = id;
        Title = title;
        Type = type;
        SubNodes = new ObservableCollection<Node>();
    }
    
    public Node(long id,string title)
    {
        Id = id;
        Title = title;
        SubNodes = new ObservableCollection<Node>();
    }

    public Node(string title)
    {
        Title = title;
        SubNodes = new ObservableCollection<Node>();
    }

    public Node(string title, ObservableCollection<Node> subNodes)
    {
        Title = title;
        SubNodes = subNodes;
    }
}

public enum NodeType
{
    Plant = 0,
    Planting,
    SeedPacket,
    People,
    Company,
    GardenBed,
    Vendor
}