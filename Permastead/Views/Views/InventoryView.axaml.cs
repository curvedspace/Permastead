using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Ursa.Controls;

namespace Permastead.Views.Views;

public partial class InventoryView : UserControl
{
    private InventoryViewModel? _viewModel;
    
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not InventoryViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
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
        var win = new InventoryGroupWindow();
        var vm = new InventoryGroupWindowViewModel(new InventoryGroup(), (InventoryViewModel)DataContext);
        
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 600;
        win.Height = 300;
        win.Opacity = 0.95;
        win.Title = "New Inventory Group";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void AddTypeMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new InventoryTypeWindow();
        var vm = new InventoryTypeWindowViewModel(new InventoryType(), (InventoryViewModel)DataContext);
        
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 600;
        win.Height = 300;
        win.Opacity = 0.95;
        win.Title = "New Inventory Type";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            
            var vm = (InventoryViewModel)this.DataContext;
            var textValue = vm.SearchText;

            if (vm != null)
            {
                vm.RefreshData(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (InventoryViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshData(vm.SearchText);
            }
        }
    }
}