using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void TreeDataGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var vm = (InventoryViewModel)DataContext;
            
            vm.CurrentItem = (Inventory)current.RowSelection!.SelectedItem;
            vm.GetInventoryObservations();
        }
    }

    private void TreeDataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var inventoryWindow = new InventoryWindow();
        
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var inventory = (Inventory)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new InventoryWindowViewModel(inventory, (InventoryViewModel)DataContext);
            
            inventoryWindow.DataContext = vm;
        
            inventoryWindow.Topmost = true;
            inventoryWindow.Width = 900;
            inventoryWindow.Height = 550;
            inventoryWindow.Opacity = 0.95;
            inventoryWindow.Title = "Inventory Item - " + inventory.Description;
        }

        inventoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        inventoryWindow.Show();
        
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new InventoryWindow();
        var vm = new InventoryWindowViewModel();
        vm.ControlViewModel = (InventoryViewModel)DataContext;
        
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 900;
        win.Height = 550;
        win.Opacity = 0.95;
        win.Title = "New Inventory Item";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void AddGroupMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void AddTypeMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }
}