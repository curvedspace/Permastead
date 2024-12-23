using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class PreservationView : UserControl
{
    public PreservationView()
    {
        InitializeComponent();
    }

    private void PreservationGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var current = sender as TreeDataGrid;
            if (current != null)
            {
                var item = (FoodPreservation)current.RowSelection!.SelectedItem;
                var vm = DataContext as PreservationViewModel;
                vm.CurrentItem = item;
                vm.GetPreservationObservations();

            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PreservationGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var item = (FoodPreservation)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new PreservationWindowViewModel(item, (PreservationViewModel)DataContext);
            
        }
        
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (PreservationViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (PreservationViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }
}