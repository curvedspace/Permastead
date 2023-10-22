using System.Collections.Generic;
using Dock.Model.Mvvm.Controls;
using DynamicData.Binding;
using Models;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Permastead.ViewModels.Views;
using CommunityToolkit.Mvvm.Input;
using Services;

namespace Permastead.ViewModels.Tools;

public partial class PeopleToolViewModel : Tool
{
    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();

    [ObservableProperty]
    private Person _currentPerson;
    
    [ObservableProperty] private bool _activeOnly = true;
    
    [ObservableProperty] private ObservableCollection<Node> _nodes;

    [ObservableProperty] private ObservableCollection<Node> _selectedNodes;

    [ObservableProperty] private List<string> _searchItems;

    [ObservableProperty] private string _searchText;


    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }

    public PeopleToolViewModel() 
    {
        RefreshData();
    }

    
    [RelayCommand]
    public void Open()
    {
        if (_selectedNodes != null && _selectedNodes.Count > 0)
        {
            if (_selectedNodes[0].Id > 0 ) _currentPerson = PersonService.GetPersonFromId(ServiceMode.Local, _selectedNodes[0].Id);         
        }
        else
        {
            //check for null
            if (_currentPerson == null)
            {
                _currentPerson = new Person();
            }
            else
            {
                _currentPerson = PersonService.GetPersonFromId(ServiceMode.Local, _currentPerson.Id);     
            }
        }

        
        if (_currentPerson != null)
            this.Dock.OpenDoc(_currentPerson);
    }
    
        [RelayCommand]
    public void RefreshData()
    {
        _searchItems = new List<string>();
        
        _currentPerson = new Person();

        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        

        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>();
        
        var peopleNode = new Node("People", new ObservableCollection<Node>());
        
        //peopleNode
        Nodes.Add(peopleNode);
        var allPeople = new Node("All", new ObservableCollection<Node>());
        peopleNode.SubNodes!.Add(allPeople);   
        
        foreach (var p in _people)
        {
            allPeople.SubNodes.Add(new Node(p.Id, p.FullNameLastFirst, NodeType.People));
            SearchItems.Add("C:" + p.Id + ": " + p.FullNameLastFirst.ToString());
        }
        
        
    }

    [RelayCommand]
    public void ResetSearchBox()
    {
        SearchText = "";
    }
    
    [RelayCommand]
    public void OpenDocument(Node node)
    {
        switch (node.Type)
        {
            case NodeType.People:
                Open();
                break;
            default:
                break;
        }
    }
    
    
    [RelayCommand]
    public void CreateNewPerson()
    {
        _currentPerson = new Person();
        _currentPerson.FirstName = "New";
        _currentPerson.LastName = "Person";
        
        this.Dock.OpenDoc(_currentPerson);
    }
    
    
    [RelayCommand]
    public void ExpandTreeNodes(TreeView treeView)
    {
        // foreach (object item in treeView.Items)
        //     if (treeView.ItemContainerGenerator.CreateContainer(item) is TreeViewItem treeItem)
        //         treeItem.IsExpanded = true;
    }
    
    public void ExpandAll()
    {
        
    }

}
