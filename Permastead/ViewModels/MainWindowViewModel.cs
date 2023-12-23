using System.Diagnostics;
using System.Windows.Input;
using Permastead.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Permastead.ViewModels.Views;

namespace Permastead.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase[] Views =
        {
            new DashboardViewModel(),
            new ObservationsViewModel(),
            new ToDoViewModel(),
            new EventsViewModel(),
            new InventoryViewModel(),
            new PlantingsViewModel(),
            new ContactsViewModel(),
            new SettingsViewModel()
        };
        

        [ObservableProperty] 
        private ViewModelBase _CurrentView;
  
    
        [ObservableProperty] 
        private string _CurrentViewName;

        [RelayCommand]
        private void OpenHomeView()
        {
            CurrentView = Views[0];
        }


        [RelayCommand]
        private void OpenObservationView()
        {
            CurrentView = Views[1];
        }
        
        [RelayCommand]
        private void OpenToDoView()
        {
            CurrentView = Views[2];
        }
        
        [RelayCommand]
        private void OpenEventsView()
        {
            CurrentView = Views[3];
        }
        
        [RelayCommand]
        private void OpenInventoryView()
        {
            CurrentView = Views[4];
        }
        
        [RelayCommand]
        private void OpenPlantingsView()
        {
            CurrentView = Views[5];
        }
        
        [RelayCommand]
        private void OpenPeopleView()
        {
            CurrentView = Views[6];
        }
        
        [RelayCommand]
        private void OpenSettingsView()
        {
            CurrentView = Views[7];
        }

        public MainWindowViewModel()
        {
            CurrentView = Views[0];
        }
        
//         private readonly IFactory? _factory;
//         private IRootDock? _layout;
//         
//         
//         public IRootDock? Layout
//         {
//             get => _layout;
//             set => SetProperty(ref _layout, value);
//         }
//
//         public ICommand NewLayout { get; }
//
//         public MainWindowViewModel()
//         {
//             _factory = new DockFactory(new DemoData());
//
//             DebugFactoryEvents(_factory);
//
//             Layout = _factory?.CreateLayout();
//             if (Layout is { })
//             {
//                 _factory?.InitLayout(Layout);
//                 if (Layout is { } root)
//                 {
//                     root.Navigate.Execute("Dashboard");
//                 }
//             }
//
//             NewLayout = new RelayCommand(ResetLayout);
//         }
//
//
//         private void DebugFactoryEvents(IFactory factory)
//         {
//             factory.ActiveDockableChanged += (_, args) =>
//             {
//                 Debug.WriteLine($"[ActiveDockableChanged] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.FocusedDockableChanged += (_, args) =>
//             {
//                 Debug.WriteLine($"[FocusedDockableChanged] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableAdded += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableAdded] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableRemoved += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableRemoved] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableClosed += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableClosed] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableMoved += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableMoved] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableSwapped += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableSwapped] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockablePinned += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockablePinned] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.DockableUnpinned += (_, args) =>
//             {
//                 Debug.WriteLine($"[DockableUnpinned] Title='{args.Dockable?.Title}'");
//             };
//
//             factory.WindowOpened += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowOpened] Title='{args.Window?.Title}'");
//             };
//
//             factory.WindowClosed += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowClosed] Title='{args.Window?.Title}'");
//             };
//
//             factory.WindowClosing += (_, args) =>
//             {
//                 // NOTE: Set to True to cancel window closing.
// #if false
//                 args.Cancel = true;
// #endif
//                 Debug.WriteLine($"[WindowClosing] Title='{args.Window?.Title}', Cancel={args.Cancel}");
//             };
//
//             factory.WindowAdded += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowAdded] Title='{args.Window?.Title}'");
//             };
//
//             factory.WindowRemoved += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowRemoved] Title='{args.Window?.Title}'");
//             };
//
//             factory.WindowMoveDragBegin += (_, args) =>
//             {
//                 // NOTE: Set to True to cancel window dragging.
// #if false
//                 args.Cancel = true;
// #endif
//                 Debug.WriteLine($"[WindowMoveDragBegin] Title='{args.Window?.Title}', Cancel={args.Cancel}, X='{args.Window?.X}', Y='{args.Window?.Y}'");
//             };
//
//             factory.WindowMoveDrag += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowMoveDrag] Title='{args.Window?.Title}', X='{args.Window?.X}', Y='{args.Window?.Y}");
//             };
//
//             factory.WindowMoveDragEnd += (_, args) =>
//             {
//                 Debug.WriteLine($"[WindowMoveDragEnd] Title='{args.Window?.Title}', X='{args.Window?.X}', Y='{args.Window?.Y}");
//             };
//         }
//
//         public void CloseLayout()
//         {
//             if (Layout is IDock dock)
//             {
//                 if (dock.Close.CanExecute(null))
//                 {
//                     dock.Close.Execute(null);
//                 }
//             }
//         }
//
//         public void ResetLayout()
//         {
//             if (Layout is not null)
//             {
//                 if (Layout.Close.CanExecute(null))
//                 {
//                     Layout.Close.Execute(null);
//                 }
//             }
//
//             var layout = _factory?.CreateLayout();
//             if (layout is not null)
//             {
//                 Layout = layout;
//                 _factory?.InitLayout(layout);
//             }
//         }
//
//         public void ExitApplication()
//         {
//             
//         }
     }

}