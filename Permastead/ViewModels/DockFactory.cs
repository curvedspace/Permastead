using System;
using System.Collections.Generic;
using System.Linq;
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
using DynamicData.Kernel;
using Models;

namespace Permastead.ViewModels;

public class DockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;

    private HomeViewModel _homeView;
    private BrowserViewModel _browser;

    private ToolDock _toolDock;

    public DockFactory(object context)
    {
        _context = context;
    }

    public override IDocumentDock CreateDocumentDock() => new CustomDocumentDock();

    public override IRootDock CreateLayout()
    {
        var document1 = new DocumentViewModel {Id = "Document1", Title = "Document1"};
        var document2 = new DocumentViewModel {Id = "Document2", Title = "Document2"};
        var document3 = new PlantDocumentViewModel {Id = "Document3", Title = "Document3", CanClose = true};
        var tool1 = new Tool1ViewModel {Id = "Tool1", Title = "Plants"};
        
        var tool3 = new Tool3ViewModel {Id = "Tool3", Title = "Tool3"};
        var tool4 = new Tool4ViewModel {Id = "Tool4", Title = "Tool4"};
        var tool5 = new Tool5ViewModel {Id = "Tool5", Title = "Tool5"};
        
        _browser = new BrowserViewModel {Id = "Browser", Title = "Browser"};

        tool1.Home = _homeView;
        tool1.Dock = this;

        _browser.Home = _homeView;
        _browser.Dock = this;

        var leftDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = tool1,
                    VisibleDockables = CreateList<IDockable>(_browser),
                    Alignment = Alignment.Left
                }
                //},
                // new ProportionalDockSplitter(),
                // new ToolDock
                // {
                //     ActiveDockable = tool3,
                //     VisibleDockables = CreateList<IDockable>(tool3, tool4),
                //     Alignment = Alignment.Bottom
                // }
            )
        };

        var rightDock = new ProportionalDock
        {
            Proportion = 0.15,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = tool5,
                    VisibleDockables = CreateList<IDockable>(tool5),
                    Alignment = Alignment.Top,
                    GripMode = GripMode.Hidden
                }
                //},
                // new ProportionalDockSplitter(),
                // new ToolDock
                // {
                //     ActiveDockable = tool7,
                //     VisibleDockables = CreateList<IDockable>(tool7, tool8),
                //     Alignment = Alignment.Right,
                //     GripMode = GripMode.AutoHide
                // }
            )
        };

        _toolDock = (ToolDock)rightDock.VisibleDockables[0];

        var documentDock = new CustomDocumentDock
        {
            IsCollapsable = false,
            ActiveDockable = document1,
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
                // new ProportionalDockSplitter(),
                // rightDock
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
        rootDock.VisibleDockables = CreateList<IDockable>(dashboardView, observationsPageView, todoPageView, eventsPageView, inventoryPageView, settingsPageView, _homeView);
        

        _documentDock = documentDock;
        _rootDock = rootDock;
            
        return rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            ["Document1"] = () => new DemoDocument(),
            ["Document2"] = () => new DemoDocument(),
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
            ["SettingsPage"] = () => layout,
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
                doc = new PlantingDocumentViewModel(currPlanting, _browser);
                doc.Title = currPlanting.Description;
                break;
            
            case "Models.SeedPacket":
                SeedPacket seedPacket = currentItem as SeedPacket;
                doc = new SeedPacketDocumentViewModel(seedPacket);
                doc.Title = seedPacket.Description;
                break;
            
            case "Models.Vendor":
                Vendor vendor = currentItem as Vendor;
                doc = new VendorDocumentViewModel(vendor, _browser);
                doc.Title = vendor.Description;
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
