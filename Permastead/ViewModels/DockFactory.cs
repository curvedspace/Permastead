using System;
using System.Collections.Generic;

using Permastead.Models.Documents;
using Permastead.Models.Tools;
using Permastead.ViewModels.Docks;
using Permastead.ViewModels.Documents;
using Permastead.ViewModels.Tools;
using Permastead.ViewModels.Views;

using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;

using Models;

namespace Permastead.ViewModels;

public class DockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;

    private HomeViewModel _homeView;
    private GreenhouseToolViewModel _greenhouseTool;
    private KitchenToolViewModel _kitchenTool;
    private PeopleToolViewModel _peopleTool;

    private ToolDock _toolDock;

    public DockFactory(object context)
    {
        _context = context;
    }

    public override IDocumentDock CreateDocumentDock() => new CustomDocumentDock();

    public override IRootDock CreateLayout()
    {
        _peopleTool = new PeopleToolViewModel {Id = "People", Title = "People"};
        _kitchenTool = new KitchenToolViewModel {Id = "Kitchen", Title = "Kitchen"};
        _greenhouseTool = new GreenhouseToolViewModel {Id = "Greenhouse", Title = "Greenhouse"};

        _peopleTool.Home = _homeView;
        _peopleTool.Dock = this;
        
        _kitchenTool.Home = _homeView;
        _kitchenTool.Dock = this;

        _greenhouseTool.Home = _homeView;
        _greenhouseTool.Dock = this;

        var leftDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Vertical,
            Title = "Workbench",
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = _greenhouseTool,
                    VisibleDockables = CreateList<IDockable>(_greenhouseTool, _peopleTool, _kitchenTool),
                    Alignment = Alignment.Left
                    
                }
            )
        };

        // var rightDock = new ProportionalDock
        // {
        //     Proportion = 0.15,
        //     Orientation = Orientation.Vertical,
        //     ActiveDockable = null,
        //     VisibleDockables = CreateList<IDockable>
        //     (
        //         new ToolDock
        //         {
        //             ActiveDockable = _kitchenTool,
        //             VisibleDockables = CreateList<IDockable>(_kitchenTool),
        //             Alignment = Alignment.Top,
        //             GripMode = GripMode.Hidden
        //         }
        //     )
        // };

        // _toolDock = (ToolDock)rightDock.VisibleDockables[0];

        var documentDock = new CustomDocumentDock
        {
            IsCollapsable = false,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>(),
            CanCreateDocument = false
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDock,
                new ProportionalDockSplitter(),
                documentDock
            )
        };

        var dashboardView = new DashboardViewModel
        {
            Id = "Dashboard",
            Title = "Dashboard"
        };

        var observationsPageView = new ObservationsPageViewModel()
        {
            Id = "ObservationsPage",
            Title = "ObservationsPage"
        };

        var todoPageView = new ToDoPageViewModel()
        {
            Id = "ToDoPage",
            Title = "ToDoPage"
        };
        
        var eventsPageView = new EventsPageViewModel()
        {
            Id = "EventsPage",
            Title = "EventsPage"
        };
        
        var inventoryPageView = new InventoryPageViewModel()
        {
            Id = "InventoryPage",
            Title = "InventoryPage"
        };
        
        var plantingsPageView = new PlantingsPageViewModel()
        {
            Id = "PlantingsPage",
            Title = "PlantingsPage"
        };
        
        var contactsPageView = new ContactsPageViewModel()
        {
            Id = "ContactsPage",
            Title = "ContactsPage"
        };
        
        var gaiaPageView = new GaiaPageViewModel()
        {
            Id = "GaiaPage",
            Title = "GaiaPage"
        };
        
        var settingsPageView = new SettingsPageViewModel()
        {
            Id = "SettingsPage",
            Title = "SettingsPage"
        };

        _homeView = new HomeViewModel
        {
            Id = "Home",
            Title = "Home",
            ActiveDockable = mainLayout,
            VisibleDockables = CreateList<IDockable>(mainLayout)
        };

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.ActiveDockable = dashboardView;
        rootDock.DefaultDockable = _homeView;
        rootDock.VisibleDockables = CreateList<IDockable>(dashboardView, observationsPageView, todoPageView, 
            eventsPageView, inventoryPageView, settingsPageView, plantingsPageView, contactsPageView, gaiaPageView, _homeView);
        

        _documentDock = documentDock;
        _rootDock = rootDock;
            
        return rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            ["Document3"] = () => new DemoDocument(),
            ["Tool1"] = () => new Tool1(),
            ["Tool2"] = () => new Tool2(),
            ["Tool3"] = () => new Tool3(),
            ["Tool4"] = () => new Tool4(),
            ["Tool5"] = () => new Tool5(),
            ["Tool6"] = () => new Tool6(),
            ["Tool7"] = () => new Tool7(),
            ["Tool8"] = () => new Tool8(),
            ["Dashboard"] = () => layout,
            ["ObservationsPage"] = () => layout,
            ["ToDoPage"] = () => layout,
            ["EventsPage"] = () => layout,
            ["InventoryPage"] = () => layout,
            ["GaiaPage"] = () => layout,
            ["SettingsPage"] = () => layout,
            ["PlantingsPage"] = () => layout,
            ["ContactsPage"] = () => layout,
            ["Home"] = () => _context
        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>()
        {
            ["Root"] = () => _rootDock,
            ["Documents"] = () => _documentDock
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }

    public void OpenDoc(Object currentItem)
    {
        Document doc = null;

        switch (currentItem.GetType().ToString())
        {
            case "Models.Plant":
                Plant currPlant = (Plant)currentItem;
                doc = new PlantDocumentViewModel(currPlant);
                doc.Title = currPlant.Description.ToString();
                doc.Context = doc;
                break;
            
            case "Models.Planting":
                Planting currPlanting = currentItem as Planting;
                doc = new PlantingDocumentViewModel(currPlanting, _greenhouseTool);
                doc.Title = currPlanting.Description;
                break;
            
            case "Models.SeedPacket":
                SeedPacket seedPacket = currentItem as SeedPacket;
                doc = new SeedPacketDocumentViewModel(seedPacket);
                doc.Title = seedPacket.Description;
                break;
            
            case "Models.Vendor":
                Vendor vendor = currentItem as Vendor;
                doc = new VendorDocumentViewModel(vendor, _greenhouseTool);
                doc.Title = vendor.Description;
                break;
            
            case "Models.GardenBed":
                GardenBed bed = currentItem as GardenBed;
                doc = new PlantingLocationDocumentViewModel(bed);
                doc.Title = bed.Description;
                break;
            
            case "Models.Person":
                Person person = currentItem as Person;
                doc = new PersonDocumentViewModel(person);
                doc.Title = person.FullNameLastFirst;
                break;
            
            default:
                doc = new DocumentViewModel();
                doc.Title = "Unknown";
                break;
        }

        if (doc != null)
        {
            _documentDock.VisibleDockables.Add(doc);
            _documentDock.ActiveDockable = doc;
            
            _toolDock.ActiveDockable.Title = doc.Title;

        }
    }
}
