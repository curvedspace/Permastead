using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Tools;
using Permastead.ViewModels.Views;
using Node = Permastead.ViewModels.Views.Node;

namespace Permastead.Views.Views;

public partial class PlantingsView : UserControl
{
    public PlantingsView()
    {
        InitializeComponent();
        DataContext = new PlantingsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var tv = this.FindControl<TreeView>("TreeBrowser");
            
            if (tv is { SelectedItems.Count: > 0 })
            {
                if (tv.SelectedItems[0] is Node node)
                {
                    Console.WriteLine(node.Title);
                    
                    //refresh plantings
                    var vm = DataContext as PlantingsViewModel;
                    vm!.RefreshData(node,"");
                }

            }
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

    }
}