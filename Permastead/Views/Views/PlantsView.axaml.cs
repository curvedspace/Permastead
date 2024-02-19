using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class PlantsView : UserControl
{
    public PlantsView()
    {
        InitializeComponent();
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        
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
}