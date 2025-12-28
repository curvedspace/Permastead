using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Models;
using Services;

using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

using Node = Permastead.ViewModels.Views.Node;
using NodeType = Permastead.ViewModels.Views.NodeType;

namespace Permastead.Views.Views;

public partial class PlantingsView : UserControl
{
    public PlantingsView()
    {
        InitializeComponent();
        DataContext = new PlantingsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var tv = this.FindControl<TreeView>("TreeBrowser");
            
            if (tv is { SelectedItems.Count: > 0 })
            {
                if (tv.SelectedItems[0] is Node node)
                {
                    Console.WriteLine(node.Title);
                    
                    //refresh plantings
                    var vm = DataContext as PlantingsViewModel;
                    vm!.RefreshData(node,"");
                }

            }
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

    }
    
    private void PlantingsGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var current = sender as TreeDataGrid;
            if (current != null)
            {
                var planting = (Planting)current.RowSelection!.SelectedItem;

                if (planting != null)
                {
                    planting.YieldRatingValue = Convert.ToDouble(planting.YieldRating / 20m);

                    var vm = DataContext as PlantingsViewModel;
                    vm.CurrentItem = planting;
                    vm.GetPlantingObservations();
                }
                
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PlantingsGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new PlantingWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var planting = (Planting)current.RowSelection!.SelectedItem;
            planting.YieldRatingValue = Convert.ToDouble(planting.YieldRating / 20m);
            
            //get underlying view's viewmodel
            var vm = new PlantingWindowViewModel(planting, (PlantingsViewModel)DataContext);
            
            plantingWindow.DataContext = vm;
        
            plantingWindow.Topmost = true;
            plantingWindow.Width = 900;
            plantingWindow.Height = 500;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Planting - " + planting.Description;
        }

        plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
        // IReadOnlyList<Window>? windows = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
        // Window? parent = windows.First();
        //
        // obsWindow.ShowDialog(parent);
            
        plantingWindow.Show();
    }

    private void NewPlantingMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var plantingWindow = new PlantingWindow();
        var vm = new PlantingWindowViewModel(new Planting(), (PlantingsViewModel)DataContext);
            
        plantingWindow.DataContext = vm;
        
        plantingWindow.Topmost = true;
        plantingWindow.Width = 875;
        plantingWindow.Height = 550;
        plantingWindow.Opacity = 0.95;
        plantingWindow.Title = "New Planting";
        plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        plantingWindow.Show();
    }

    private void NewPlantMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var plantWindow = new PlantWindow();
        var vm = new PlantWindowViewModel();
            
        plantWindow.DataContext = vm;
        
        plantWindow.Topmost = true;
        plantWindow.Width = 900;
        plantWindow.Height = 700;
        plantWindow.Opacity = 0.95;
        plantWindow.Title = "New Plant";
        plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        plantWindow.Show();
    }
    
    private void NewLocationMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new LocationWindow();
        var vm = new LocationWindowViewModel();
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 700;
        win.Height = 370;
        win.Opacity = 0.95;
        win.Title = "New Location";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
    
    private void NewStarterMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new StarterWindow();
        var vm = new StarterWindowViewModel();
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 800;
        win.Height = 650;
        win.Opacity = 0.95;
        win.Title = "New Plant Starter";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
    
    private void NewVendorMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new VendorWindow();
        var vm = new VendorWindowViewModel(new Vendor(), (PlantingsViewModel)DataContext);
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 500;
        win.Height = 300;
        win.Opacity = 0.95;
        win.Title = "New Vendor";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void EditMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var plantWindow = new PlantWindow();
        var locWindow = new LocationWindow();
        var vendorWindow = new VendorWindow();

        Plant currentPlant;
        var tree = this.FindControl<TreeView>("TreeBrowser");
        Node currentNode = tree.SelectedItem as Node;
        
        // figure out what to do based on the node type
        if (currentNode != null)
        {
            switch (currentNode.Type)
            {
                case NodeType.Plant:
                    var plant = PlantService.GetPlantFromId(AppSession.ServiceMode, currentNode.Id);
                    var plantWindowViewModel = new PlantWindowViewModel(plant, (PlantingsViewModel)DataContext);
                    plantWindowViewModel.Plant = plant;
                    plantWindow.DataContext = plantWindowViewModel;
        
                    plantWindow.Topmost = true;
                    plantWindow.Width = 900;
                    plantWindow.Height = 700;
                    plantWindow.Opacity = 0.95;
                    plantWindow.Title = "Edit Plant";
                    plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                    plantWindow.Show();
                    break;
                
                case NodeType.GardenBed:
                    var bed = PlantingsService.GetGardenBedFromId(AppSession.ServiceMode, currentNode.Id);
                    var locationWindowViewModel = new LocationWindowViewModel(bed, (PlantingsViewModel)DataContext);
                    locationWindowViewModel.Bed = bed;
                    locWindow.DataContext = locationWindowViewModel;
        
                    locWindow.Topmost = true;
                    locWindow.Width = 700;
                    locWindow.Height = 370;
                    locWindow.Opacity = 0.95;
                    locWindow.Title = "Edit Bed Location";
                    locWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                    locWindow.Show();
                    break;
                
                case NodeType.Vendor:
                    var vendors = VendorService.GetAll(AppSession.ServiceMode);
                    var vendor = vendors.FirstOrDefault(x => x.Id == currentNode.Id);
                    var vendorWindowViewModel = new VendorWindowViewModel(vendor, (PlantingsViewModel)DataContext);
                    vendorWindowViewModel.Vendor = vendor;
                    vendorWindow.DataContext = vendorWindowViewModel;
        
                    vendorWindow.Topmost = true;
                    vendorWindow.Width = 500;
                    vendorWindow.Height = 300;
                    vendorWindow.Opacity = 0.95;
                    vendorWindow.Title = "Edit Vendor";
                    vendorWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                    vendorWindow.Show();
                    break;
            }
        }
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var vm = (PlantingsViewModel)this.DataContext;
            var textValue = vm.SearchText;

            if (vm != null)
            {
                vm.RefreshData(null,textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (PlantingsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshData(null,vm.SearchText);
            }
        }
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        
    }
}