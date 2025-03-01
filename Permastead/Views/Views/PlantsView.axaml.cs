using System;
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

public partial class PlantsView : UserControl
{
    public PlantsView()
    {
        InitializeComponent();
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        // add a new plant via a dialog
        try
        {
            PlantsViewModel plantVm  = (PlantsViewModel)DataContext;
            
            
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
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PlantsList_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var currentItem = sender as ListBox;
            var current = currentItem.SelectedValue as Plant;
           
            var vm = (PlantsViewModel)DataContext;
            vm.CurrentPlant = current;
            vm.GetMetaData();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            PlantsViewModel vm  = (PlantsViewModel)DataContext;
            vm.SavePlant();

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
            var vm = (PlantsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (PlantsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        
    }
}