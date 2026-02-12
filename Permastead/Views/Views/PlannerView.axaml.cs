using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class PlannerView : UserControl
{
    public PlannerView()
    {
        InitializeComponent();
    }

    private void PlantsListBox_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var currentItem = sender as ListBox;
            var current = currentItem.SelectedValue as Plant;
           
            var vm = (PlannerViewModel)DataContext;
            vm.CurrentPlant = current;
            vm.RefreshDataOnly();
            vm.UpdateSeedPacketsCommand.Execute(null);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}