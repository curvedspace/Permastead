using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ProceduresView : UserControl
{
    public ProceduresView()
    {
        InitializeComponent();
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
            //var currentItem = sender as ItemsRepeater;
            //var current = currentItem.Get as StandardOperatingProcedure;
            
            //var vm = (ProceduresViewModel)DataContext;
            //vm.CurrentItem = current;
            
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