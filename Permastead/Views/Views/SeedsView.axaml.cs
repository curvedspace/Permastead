using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class SeedsView : UserControl
{
    public SeedsView()
    {
        InitializeComponent();
        DataContext = new SeedsViewModel();
    }

    private void SeedsGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new StarterWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var seedPacket = (SeedPacket)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            // var vm = new StarterWindowViewModel(seedPacket, (SeedsViewModel)DataContext);
            var vm = new StarterWindowViewModel(seedPacket);
            vm.ControlViewModel = (SeedsViewModel)DataContext;
            
            plantingWindow.DataContext = vm;
        
            plantingWindow.Topmost = true;
            plantingWindow.Width = 1000;
            plantingWindow.Height = 550;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Starter - " + seedPacket.Description;
        }

        plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
        // IReadOnlyList<Window>? windows = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
        // Window? parent = windows.First();
        //
        // obsWindow.ShowDialog(parent);
            
        plantingWindow.Show();
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        Console.WriteLine(sender.ToString() + ": " + e);
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (SeedsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (SeedsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }
}