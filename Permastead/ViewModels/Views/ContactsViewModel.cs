using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace Permastead.ViewModels.Views;

public partial class ContactsViewModel : ViewModelBase
{
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty] 
    private long _peopleCount;

    public FlatTreeDataGridSource<Person> PersonSource { get; set; }
    
    public ContactsViewModel()
    {
        RefreshDataOnly();
    }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }

    public void RefreshDataOnly()
    {
        var myPeople = Services.PersonService.GetAllPeople(AppSession.ServiceMode);

        _people.Clear();
        foreach (var p in myPeople)
        {
            _people.Add(p);
        }
        
        PersonSource = new FlatTreeDataGridSource<Person>(_people)
        {
            Columns =
            {
                new TextColumn<Person, string>
                    ("Last Name", x => x.LastName),
                new TextColumn<Person, string>
                    ("First Name", x => x.FirstName),
                new TextColumn<Person, string>
                    ("Company", x => x.Company),
                new TextColumn<Person, string>
                    ("Email", x => x.Email),
                new TextColumn<Person, string>
                    ("Phone", x => x.Phone),
                new TextColumn<Person, string>
                    ("Comment", x => x.Comment)
            },
        };

        PeopleCount = _people.Count;
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.
        

    }
    
}