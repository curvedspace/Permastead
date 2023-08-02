using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Tools;
using System;
using Avalonia.Interactivity;
using Models;

namespace Permastead.Views.Tools;

public partial class BrowserView : UserControl
{
    public BrowserView()
    {
        InitializeComponent();
    }

    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            Node node = (Node)TreeBrowser.SelectedItem;
            ((BrowserViewModel)(this.DataContext)).OpenDocument(node);
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

    private void SearchBox_OnDropDownClosed(object? sender, EventArgs e)
    {
        var textValue = SearchBox.Text;
        var parsedValues = textValue.Split(":");

        if (parsedValues.Length >= 3)
        {
            string itemType = parsedValues[0];
            long id = Convert.ToInt64(parsedValues[1]);
            string itemName = parsedValues[2];
            
            //create the treenode
            var node = new Node(id, itemName, NodeType.Planting);
            ((BrowserViewModel)(this.DataContext)).CurrentPlanting = new Planting() {Id = id};
            
            ((BrowserViewModel)(this.DataContext)).OpenDocument(node);

        }
        
    }
}
