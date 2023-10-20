using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Tools;
using System;
using Avalonia.Interactivity;
using Models;

namespace Permastead.Views.Tools;

public partial class GreenhouseToolView : UserControl
{
    public GreenhouseToolView()
    {
        InitializeComponent();
    }

    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            Node node = (Node)TreeBrowser.SelectedItem;
            ((GreenhouseToolViewModel)(this.DataContext)).OpenDocument(node);
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
    

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
        {
            
            var textValue = SearchBox.Text;
            var parsedValues = textValue.Split(":");

            if (parsedValues.Length >= 3 && e.Key == Key.Return)
            {
                string itemType = parsedValues[0];
                long id = Convert.ToInt64(parsedValues[1]);
                string itemName = parsedValues[2];

                NodeType nodeType = NodeType.Planting;
                
                switch (itemType)
                {
                    case "PG":
                        nodeType = NodeType.Planting;
                        ((GreenhouseToolViewModel)(this.DataContext)).CurrentPlanting = new Planting() {Id = id};
                        break;
                    case "P":
                        nodeType = NodeType.Plant;
                        ((GreenhouseToolViewModel)(this.DataContext)).CurrentPlant = new Plant() {Id = id};
                        break;
                    case "S":
                        nodeType = NodeType.SeedPacket;
                        ((GreenhouseToolViewModel)(this.DataContext)).CurrentSeedPacket = new SeedPacket() {Id = id};
                        break;
                }
                
                //create the treenode
                var node = new Node(id, itemName, nodeType);

                ((GreenhouseToolViewModel)(this.DataContext)).OpenDocument(node);

            }
        }
    }
}
