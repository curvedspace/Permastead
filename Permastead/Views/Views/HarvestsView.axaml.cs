using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class HarvestsView : UserControl
{
    public HarvestsView()
    {
        InitializeComponent();
    }

    private void HarvestsGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var current = sender as TreeDataGrid;
            if (current != null)
            {
                var item = (Harvest)current.RowSelection!.SelectedItem;
                var vm = DataContext as HarvestsViewModel;
                vm.CurrentItem = item;

            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (HarvestsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (HarvestsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        var win = new HarvestWindow();
        var vm = new HarvestWindowViewModel();
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 1000;
        win.Height = 550;
        win.Opacity = 0.95;
        win.Title = "New Harvest";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }

    private void HarvestGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var harvestWindow = new HarvestWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var item = (Harvest)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new HarvestWindowViewModel(item, (HarvestsViewModel)DataContext);
            
            harvestWindow.DataContext = vm;
        
            harvestWindow.Topmost = true;
            harvestWindow.Width = 1000;
            harvestWindow.Height = 550;
            harvestWindow.Opacity = 0.95;
            harvestWindow.Title = "Harvest - " + item.Description;
        }

        harvestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        harvestWindow.Show();
    }
}