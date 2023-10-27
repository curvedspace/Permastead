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
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private long _peopleCount;

    public FlatTreeDataGridSource<Person> PersonSource { get; }
    
    public ContactsViewModel()
    {
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));

        PersonSource = new FlatTreeDataGridSource<Person>(_people)
        {
            Columns =
            {
                new TextColumn<Person, string>
                    ("Last Name", x => x.LastName),
                new TextColumn<Person, string>
                    ("First Name", x => x.FirstName),
            },
        };
        
        // PersonSource.Columns.Add(new TextColumn<Person, string>("Last Name", x => x.LastName));
        // PersonSource.Columns.Add(new TextColumn<Person, string>("First Name", x => x.FirstName));

        PeopleCount = _people.Count;
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.

        

    }
    
}