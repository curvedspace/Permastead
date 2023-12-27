using System;

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

    private void PlantingsGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new PlantingWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var planting = (Planting)current.RowSelection!.SelectedItem;
            
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
        plantingWindow.Width = 900;
        plantingWindow.Height = 500;
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
        win.Width = 900;
        win.Height = 700;
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
        win.Width = 900;
        win.Height = 700;
        win.Opacity = 0.95;
        win.Title = "New Plant Starter";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
    
    private void NewVendorMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new VendorWindow();
        var vm = new VendorWindowViewModel();
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 900;
        win.Height = 700;
        win.Opacity = 0.95;
        win.Title = "New Vendor";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void EditMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var plantWindow = new PlantWindow();

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
                    var vm = new PlantWindowViewModel(plant, (PlantingsViewModel)DataContext);
                    vm.Plant = plant;
                    plantWindow.DataContext = vm;
        
                    plantWindow.Topmost = true;
                    plantWindow.Width = 900;
                    plantWindow.Height = 700;
                    plantWindow.Opacity = 0.95;
                    plantWindow.Title = "Edit Plant";
                    plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                    plantWindow.Show();
                    break;
                
                case NodeType.GardenBed:
                    break;
            }
        }
    }
}