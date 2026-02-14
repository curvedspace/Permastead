using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;
using Ursa.Controls;

namespace Permastead.Views.Views;

public partial class PlannerView : UserControl
{
    private PlannerViewModel? _viewModel;
    
    public PlannerView()
    {
        InitializeComponent();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not PlannerViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
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