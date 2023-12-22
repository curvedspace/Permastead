using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Tools;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Node = Permastead.ViewModels.Views.Node;

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
            var vm = new PlantingWindowViewModel(planting);
            plantingWindow.DataContext = vm;
        
            plantingWindow.Topmost = true;
            plantingWindow.Width = 900;
            plantingWindow.Height = 500;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Planting - " + planting.Description;
        }

        plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
        // IReadOnlyList<Window>? windows = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
        // Window? parent = windows.First();
        //
        // obsWindow.ShowDialog(parent);
            
        plantingWindow.Show();
    }
}