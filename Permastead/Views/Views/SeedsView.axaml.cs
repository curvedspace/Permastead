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
            plantingWindow.Width = 900;
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
}