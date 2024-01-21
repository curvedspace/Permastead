using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
        DataContext = new DashboardViewModel();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // refresh the display when the planting year has changed
        try
        {
            var vm = (DashboardViewModel)DataContext;
            vm.RefreshDataOnly();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}