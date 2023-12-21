using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class ObservationsView : UserControl
{
    public ObservationsView()
    {
        InitializeComponent();
        DataContext = new ObservationsViewModel();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ObservationsList_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        //get the selected row in the list
        var currentObs = sender as ListBox;
        var obs = currentObs.SelectedValue as Observation;
        
        var obsWindow = new ObservationWindow();

        obsWindow.Topmost = true;
        obsWindow.Width = 250;
        obsWindow.Height = 200;
        obsWindow.Show();
    }
}
