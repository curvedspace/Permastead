using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Tools;
using System;
using Avalonia.Interactivity;

namespace Permastead.Views.Tools;

public partial class Tool2View : UserControl
{
    public Tool2View()
    {
        InitializeComponent();
    }

    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            ((Tool2ViewModel)(this.DataContext)).OpenPlanting();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);

        }

    }

    public void ExpandAllNodes(object source, RoutedEventArgs args)
    {
        foreach (var tvi in TreeBrowser.Items)
        {
            //TreeViewItem i = TreeBrowser.ItemContainerGenerator;
            Console.WriteLine(tvi.ToString());
        }
        
        
    }
}
