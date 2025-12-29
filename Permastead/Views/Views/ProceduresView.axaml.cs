using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;
using Ursa.Controls;

namespace Permastead.Views.Views;

public partial class ProceduresView : UserControl
{
    private ProceduresViewModel? _viewModel;
    
    public ProceduresView()
    {
        InitializeComponent();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not ProceduresViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
    }
    
    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (ProceduresViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (ProceduresViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            
            var currentItem = sender as ListBox;
            var current = currentItem.SelectedValue as StandardOperatingProcedure;
           
            var vm = (ProceduresViewModel)DataContext;
            vm.CurrentItem = current;
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}