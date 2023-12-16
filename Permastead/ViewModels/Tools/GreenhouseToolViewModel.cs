using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Avalonia.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Dock.Model.Mvvm.Controls;
using DynamicData;
using Models;
using Services;

using Permastead.ViewModels.Views;

namespace Permastead.ViewModels.Tools;

public partial class GreenhouseToolViewModel : Tool
{

    [ObservableProperty] private Planting _currentPlanting;
    
    [ObservableProperty] private Plant _currentPlant;
    
    [ObservableProperty] private SeedPacket _currentSeedPacket;
    
    [ObservableProperty] private Vendor _currentVendor;

    [ObservableProperty] private GardenBed _currentPlantingLocation;

    [ObservableProperty] private bool _activeOnly = true;

    private ObservableCollection<Planting> _plantings;
    private ObservableCollection<Plant> _plants;
    private ObservableCollection<Inventory> _inventory;
    private ObservableCollection<SeedPacket> _seedsPackets;
    private ObservableCollection<GardenBed> _beds;
    private ObservableCollection<AnEvent> _events;

    [ObservableProperty] private ObservableCollection<Node> _nodes;

    [ObservableProperty] private ObservableCollection<Node> _selectedNodes;

    [ObservableProperty] private List<string> _searchItems;

    [ObservableProperty] private string _searchText;

    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }

    public GreenhouseToolViewModel()
    {
        RefreshData();
    }

    [RelayCommand]
    public void RefreshData()
    {
        _searchItems = new List<string>();
        
        _plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode));
        _plants = new ObservableCollection<Plant>(PlantService.GetAllPlants(AppSession.ServiceMode));
        _inventory = new ObservableCollection<Inventory>(InventoryService.GetAllInventory(AppSession.ServiceMode));
        _seedsPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
        _beds = new ObservableCollection<GardenBed>(PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        _events = new ObservableCollection<AnEvent>(EventsService.GetAllEvents(AppSession.ServiceMode));
        
        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>();
        
        var plantsNode = new Node("Plants (" + _plants.Count + ")", new ObservableCollection<Node>());
        var seedsNode = new Node("Seeds (" + _seedsPackets.Count + ")", new ObservableCollection<Node>());
        var plantingsNode = new Node("Plantings", new ObservableCollection<Node>());
        var inventoryNode = new Node("Inventory", new ObservableCollection<Node>());
        var contactsNode = new Node("Contacts", new ObservableCollection<Node>());
        var eventsNode = new Node("Events", new ObservableCollection<Node>());

        //plantings
        Nodes.Add(plantingsNode);
        var plantingsCount = 0;
        var allPlantings = new Node("All (" + _plantings.Count + ")", new ObservableCollection<Node>());
        plantingsNode.SubNodes!.Add(allPlantings);   
        
        foreach (var p in _plantings)
        {
            if (ActiveOnly)
            {
                if (p.IsActive)
                {
                    allPlantings.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Planting));
                    SearchItems.Add("PG:" + p.Id + ": " + p.Description.ToString());
                    plantingsCount++;
                }
                
            }
            else
            {
                allPlantings.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Planting));
                SearchItems.Add("PG:" + p.Id + ": " + p.Description.ToString());
                plantingsCount++;
            }
            
        }

        allPlantings.Title = "All (" + plantingsCount + ")";
        
        var byBedPlantings = new Node("By Location (" + _beds.Count + ")", new ObservableCollection<Node>());
        plantingsNode.SubNodes.Add(byBedPlantings);   
        foreach (var gb in _beds)
        {
            var currentBed = new Node(gb.Id, gb.Code + ": " + gb.Description, NodeType.Planting);
            byBedPlantings.SubNodes.Add(currentBed);
            //load up plantings by bed
            foreach (var p in _plantings)
            {
                if (p.Bed.Id == gb.Id)
                {
                    if (ActiveOnly)
                    {
                        if (p.IsActive) currentBed.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Planting));
                    }
                    else
                    {
                        currentBed.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Planting));
                    }
                }
            }
        }
        
        //plants
        Nodes.Add(plantsNode);
        foreach (var p in _plants)
        {
            plantsNode.SubNodes.Add(new Node(p.Id, p.Description, NodeType.Plant));
            SearchItems.Add("P:" + p.Id + ": " + p.Description.ToString());
        }
        
        //seeds
        var seedsCount = 0;
        Nodes.Add(seedsNode);
        foreach (var p in _seedsPackets)
        {
            if (ActiveOnly)
            {
                if (p.IsCurrent())
                {
                    seedsNode.SubNodes.Add(new Node(p.Id, p.Description, NodeType.SeedPacket));
                    SearchItems.Add("S:" + p.Id + ": " + p.Description.ToString());
                    seedsCount++;
                }
            }
            else
            {
                seedsNode.SubNodes.Add(new Node(p.Id, p.Description, NodeType.SeedPacket));
                SearchItems.Add("S:" + p.Id + ": " + p.Description.ToString());
                seedsCount++;
            }
        }
        
        seedsNode.Title = "Seeds (" + seedsCount + ")";
        
    }

    [RelayCommand]
    public void ResetSearchBox()
    {
        SearchText = "";
        _currentPlant = null;
        _currentPlanting = null;
        _currentSeedPacket = null;
        _currentPlantingLocation = null;
    }
    
    [RelayCommand]
    public void OpenDocument(Node node)
    {
        switch (node.Type)
        {
            case NodeType.Plant:
                OpenPlant();
                break;
            case NodeType.Planting:
                OpenPlanting();
                break;
            case NodeType.SeedPacket:
                OpenSeedPacket();
                break;
            default:
                break;
        }
    }
    
    [RelayCommand]
    public void OpenPlant()
    {
        if (_selectedNodes != null && _selectedNodes.Count > 0)
        {
            if (_selectedNodes[0].Id > 0 ) _currentPlant = PlantService.GetPlantFromId(ServiceMode.Local, _selectedNodes[0].Id);         
        }
        else
        {
            //check for null
            if (_currentPlant == null)
            {
                _currentPlant = new Plant();
            }
            else
            {
                _currentPlant = PlantService.GetPlantFromId(ServiceMode.Local, _currentPlant.Id);     
            }
        }

        if (_currentPlant != null)
            this.Dock.OpenDoc(_currentPlant);
    }
    
    [RelayCommand]
    public void OpenPlanting()
    {
        if (_selectedNodes != null && _selectedNodes.Count > 0)
        {
            _currentPlanting = PlantingsService.GetPlantingFromId(ServiceMode.Local, _selectedNodes[0].Id);         
        }
        else
        {
            //check for null
            if (_currentPlanting == null)
            {
                _currentPlanting = new Planting();
            }
            else
            {
                _currentPlanting = PlantingsService.GetPlantingFromId(ServiceMode.Local, _currentPlanting.Id);     
            }
        }

        if (_currentPlanting != null)
            this.Dock.OpenDoc(_currentPlanting);
    }

    [RelayCommand]
    public void OpenSeedPacket()
    {
        if (_selectedNodes != null && _selectedNodes.Count > 0)
        {
            if (_selectedNodes[0].Id > 0 ) _currentSeedPacket = PlantingsService.GetSeedPacketFromId(ServiceMode.Local, _selectedNodes[0].Id);         
        }
        else
        {
            //check for null
            if (_currentSeedPacket == null)
            {
                _currentSeedPacket = new SeedPacket();
            }
            else
            {
                _currentSeedPacket = PlantingsService.GetSeedPacketFromId(ServiceMode.Local, _currentSeedPacket.Id);     
            }
            
        }

        if (_currentSeedPacket != null)
            this.Dock.OpenDoc(_currentSeedPacket);
    }
    
    [RelayCommand]
    public void CreateNewPlanting()
    {
        _currentPlanting = new Planting();
        _currentPlanting.Description = "New Planting";
        
        this.Dock.OpenDoc(_currentPlanting);
    }
    
    [RelayCommand]
    public void CreateNewSeedPacket()
    {
        _currentSeedPacket = new SeedPacket();
        _currentSeedPacket.Description = "New Seeds";
        
        this.Dock.OpenDoc(_currentSeedPacket);
    }
    
    [RelayCommand]
    public void CreateNewVendor()
    {
        CurrentVendor = new Vendor();
        CurrentVendor.Description = "New Vendor";
        
        this.Dock.OpenDoc(CurrentVendor);
    }
    
    [RelayCommand]
    public void CreateNewPlant()
    {
        CurrentPlant = new Plant();
        CurrentPlant.Description = "New Plant";
        
        this.Dock.OpenDoc(CurrentPlant);
    }

    [RelayCommand]
    public void CreateNewPlantLocation()
    {
        CurrentPlantingLocation = new GardenBed();
        CurrentPlantingLocation.Description = "New Location";

        this.Dock.OpenDoc(CurrentPlantingLocation);
    }
    
    [RelayCommand]
    public void ExpandTreeNodes(TreeView treeView)
    {
        // foreach (object item in treeView.Items)
        //     if (treeView.ItemContainerGenerator.CreateContainer(item) is TreeViewItem treeItem)
        //         treeItem.IsExpanded = true;
    }
    
    public void ExpandAll()
    {
        
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
    Company
}
