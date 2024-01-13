using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Models;
using Serilog;
using Services;

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
    
    [ObservableProperty] 
    private bool _currentOnly = true;   //show only current plantings by default
    
    [ObservableProperty] 
    private Planting _currentItem;
    
    [ObservableProperty] private ObservableCollection<Node> _nodes;

    [ObservableProperty] private ObservableCollection<Node> _selectedNodes;
    
    public FlatTreeDataGridSource<Planting> PlantingsSource { get; set; }
    
    public PlantingsViewModel()
    {
        try
        {
            _beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
            _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, false));
            _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            _vendors = new ObservableCollection<Vendor>(VendorService.GetAll(AppSession.ServiceMode));

            _currentItem = new Planting();

            RefreshPlantings();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting plantings.");
        }
            
    }

    [RelayCommand]
    private void RefreshPlantings()
    {
        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>();
        
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
    private void RefreshDataOnly()
    {
        RefreshData();
    }
    
    public void RefreshData(Node node = null, string searchText = "")
    {
        // clear out plantings, prepare for filtering
        Plantings.Clear();
        
        var p1 = Services.PlantingsService.GetPlantingsByPlantedDate(AppSession.ServiceMode);

        bool addRecord = false;
        foreach (var o in p1)
        {
            addRecord = false;

            if (node != null)
            {
                switch (node.Type)
                {
                    case NodeType.Plant:
                        if (o.Plant.Description == node.Title) addRecord = true;
                        break;
                    case NodeType.GardenBed:
                        if (o.Bed.Code + ": " + o.Bed.Description == node.Title) addRecord = true;
                        break;
                    case NodeType.Vendor:
                        if (o.SeedPacket.Vendor.Description == node.Title) addRecord = true;
                        break;
                    default:
                        addRecord = false;
                        break;
                }
            }
            else
            {
                addRecord = true;
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
        //if there is a comment, save it.

        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {

            CurrentItem.CreationDate = DateTime.Now;
 
            var rtnValue = DataAccess.Local.PlantingsRepository.Insert(CurrentItem);
            
            if (rtnValue)
            {
                Plantings.Add(CurrentItem);
            }
            
            Console.WriteLine("saved " + rtnValue);

            RefreshPlantings();
        }
        else
        {
            var rtnValue = DataAccess.Local.PlantingsRepository.Update(CurrentItem);
            RefreshPlantings();
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