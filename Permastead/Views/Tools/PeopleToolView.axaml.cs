using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.Models.Tools;
using Avalonia.Interactivity;
using Permastead.ViewModels.Tools;

namespace Permastead.Views.Tools;

public partial class PeopleToolView : UserControl
{
    public PeopleToolView()
    {
        InitializeComponent();
    }
    
    private void PeopleListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            ((PeopleToolViewModel)(this.DataContext)).Open();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        
    }
    
    private void TreeBrowser_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            Node node = (Node)TreeBrowser.SelectedItem;
            ((PeopleToolViewModel)(this.DataContext)).OpenDocument(node);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
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
                    case "C":
                        nodeType = NodeType.People;
                        ((PeopleToolViewModel)(this.DataContext)).CurrentPerson = new Person() {Id = id};
                        break;
                    
                }
                
                //create the treenode
                var node = new Node(id, itemName, nodeType);
                
                if (((PeopleToolViewModel)(this.DataContext)).SelectedNodes.Count > 0) 
                    ((PeopleToolViewModel)(this.DataContext)).SelectedNodes[0] = node;

                ((PeopleToolViewModel)(this.DataContext)).OpenDocument(node);

            }
        }
    }
}
