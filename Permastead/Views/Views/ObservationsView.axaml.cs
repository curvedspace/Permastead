using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Models;
using Permastead.ViewModels.Dialogs;
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
        try
        {
            //get the selected row in the list
            var currentObs = sender as ListBox;
            var obs = currentObs.SelectedValue as Observation;
            
            //get underlying view's viewmodel
            
            var vm = new ObservationWindowViewModel(obs, (ObservationsViewModel)DataContext);
        
            var obsWindow = new ObservationWindow();
            obsWindow.DataContext = vm;
        
            obsWindow.Topmost = true;
            obsWindow.Width = 550;
            obsWindow.Height = 350;
            obsWindow.Opacity = 0.9;
            obsWindow.Title = "Observation";
            obsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            // IReadOnlyList<Window>? windows = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
            // Window? parent = windows.First();
            //
            // obsWindow.ShowDialog(parent);
            
            obsWindow.Show();
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
        
    }
}
