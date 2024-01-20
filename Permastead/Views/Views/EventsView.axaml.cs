using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class EventsView : UserControl
{
    public EventsView()
    {
        InitializeComponent();
        DataContext = new EventsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void EventsGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var vm = (EventsViewModel)DataContext;
            vm.CurrentItem = (AnEvent)current.RowSelection!.SelectedItem;
        }
    }

    private void EventsGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var eventWindow = new EventsWindow();
        
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var anEvent = (AnEvent)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new EventsWindowViewModel(anEvent, (EventsViewModel)DataContext);
            
            eventWindow.DataContext = vm;
        
            eventWindow.Topmost = true;
            eventWindow.Width = 700;
            eventWindow.Height = 450;
            eventWindow.Opacity = 0.95;
            eventWindow.Title = "Event - " + anEvent.Description;
        }

        eventWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        eventWindow.Show();
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new EventsWindow();
        var vm = new EventsWindowViewModel();
        vm.ControlViewModel = (EventsViewModel)DataContext;
        
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 700;
        win.Height = 450;
        win.Opacity = 0.95;
        win.Title = "New Event";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
}