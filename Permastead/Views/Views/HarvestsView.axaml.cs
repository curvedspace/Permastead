using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class HarvestsView : UserControl
{
    public HarvestsView()
    {
        InitializeComponent();
    }

    private void HarvestsGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        var win = new HarvestWindow();
        var vm = new HarvestWindowViewModel();
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 900;
        win.Height = 550;
        win.Opacity = 0.95;
        win.Title = "New Harvest";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
}