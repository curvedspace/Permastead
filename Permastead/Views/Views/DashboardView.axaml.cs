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
        
        // set the xaxis here, it does not work from XAML for some reason
        var vm = (DashboardViewModel)DataContext;
        MyChart.XAxes = vm.XAxes;
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // refresh the display when the planting year has changed
        try
        {
            var vm = (DashboardViewModel)DataContext;
            vm.RefreshDataOnly();

            if (vm.PlantingYear == "ALL")
            {
                YearInReviewLabel.Content = "All Years";
;            }
            else
            {
                YearInReviewLabel.Content = "Year In Review: " + vm.PlantingYear;
            }
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}