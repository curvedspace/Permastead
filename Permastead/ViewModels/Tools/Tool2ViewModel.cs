using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using DynamicData;
using Models;
using Permastead.ViewModels.Views;
using Services;

namespace Permastead.ViewModels.Tools;

public partial class Tool2ViewModel : Tool
{

    [ObservableProperty]
    private Planting _currentPlanting;

    private ObservableCollection<Planting> _plantings;
    private ObservableCollection<Plant> _plants;
    private ObservableCollection<Inventory> _inventory;
    private ObservableCollection<SeedPacket> _seedsPackets;
    private ObservableCollection<GardenBed> _beds;
    private ObservableCollection<AnEvent> _events;

    [ObservableProperty] private ObservableCollection<Node> _nodes;

    [ObservableProperty] private ObservableCollection<Node> _selectedNodes;

    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }

    public Tool2ViewModel()
    {
        RefreshData();
    }


    public void RefreshData()
    {
        _plantings = new ObservableCollection<Planting>(PlantingsService.GetPlantings(AppSession.ServiceMode));
        _plants = new ObservableCollection<Plant>(PlantService.GetAllPlants(AppSession.ServiceMode));
        _inventory = new ObservableCollection<Inventory>(InventoryService.GetAllInventory(AppSession.ServiceMode));
        _seedsPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
        _beds = new ObservableCollection<GardenBed>(PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        _events = new ObservableCollection<AnEvent>(EventsService.GetAllEvents(AppSession.ServiceMode));
        
        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>();
        
        var plantsNode = new Node("Plants", new ObservableCollection<Node>());
        var seedsNode = new Node("Seeds", new ObservableCollection<Node>());
        var plantingsNode = new Node("Plantings", new ObservableCollection<Node>());
        var inventoryNode = new Node("Inventory", new ObservableCollection<Node>());
        var contactsNode = new Node("Contacts", new ObservableCollection<Node>());
        var eventsNode = new Node("Events", new ObservableCollection<Node>());

        //plantings
        Nodes.Add(plantingsNode);
        var allPlantings = new Node("All", new ObservableCollection<Node>());
        plantingsNode.SubNodes.Add(allPlantings);   
        foreach (var p in _plantings)
        {
            allPlantings.SubNodes.Add(new Node(p.Id, p.Description));
        }
        
        var byBedPlantings = new Node("By Bed", new ObservableCollection<Node>());
        plantingsNode.SubNodes.Add(byBedPlantings);   
        foreach (var gb in _beds)
        {
            var currentBed = new Node(gb.Id, gb.Code + ": " + gb.Description);
            byBedPlantings.SubNodes.Add(currentBed);
            //load up plantings by bed
            foreach (var p in _plantings)
            {
                if (p.Bed.Id == gb.Id)
                {
                    currentBed.SubNodes.Add(new Node(p.Id, p.Description));
                }
            }
        }
        
        //plants
        Nodes.Add(plantsNode);
        foreach (var p in _plants)
        {
            plantsNode.SubNodes.Add(new Node(p.Id, p.Description));
        }
        
        Nodes.Add(seedsNode);
        foreach (var p in _seedsPackets)
        {
            seedsNode.SubNodes.Add(new Node(p.Id, p.Description));
        }
        
        //events
        Nodes.Add(eventsNode);
        foreach (var e in _events)
        {
            eventsNode.SubNodes.Add((new Node(e.Id, e.Description)));
        }

    }
    
    [RelayCommand]
    public void OpenPlanting()
    {
        if (_selectedNodes != null && _selectedNodes.Count > 0)
        {
            _currentPlanting = PlantingsService.GetPlantingFromId(ServiceMode.Local, _selectedNodes[0].Id);         
        }

        if (_currentPlanting != null)
            this.Dock.OpenDoc(_currentPlanting);
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
    public string Title { get; }

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
