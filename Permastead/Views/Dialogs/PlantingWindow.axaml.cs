using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class PlantingWindow : Window
{
    public PlantingWindow()
    {
        InitializeComponent();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantingWindowViewModel vm  = (PlantingWindowViewModel)DataContext;
        vm.SavePlanting();
        
        Close();
    }

    private void HarvestButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantingWindowViewModel vm  = (PlantingWindowViewModel)DataContext;
        vm.HarevstPlanting();
        
        Close();
    }

    private void TerminateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantingWindowViewModel vm  = (PlantingWindowViewModel)DataContext;
        vm.TerminatePlanting();
        
        Close();
    }
}