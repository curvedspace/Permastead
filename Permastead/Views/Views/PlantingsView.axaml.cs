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
    
    private void TreeBrowser_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            Node node = (Node)TreeBrowser.SelectedItem;
            if (node != null) Console.WriteLine(node.Title);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

    }
}