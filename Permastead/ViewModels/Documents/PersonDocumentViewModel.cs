using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using Models;
using Serilog;

namespace Permastead.ViewModels.Documents;

public partial class PersonDocumentViewModel : Document
{
    [ObservableProperty] 
    private long _personCount;

    [ObservableProperty] 
    private Person _person;


    public PersonDocumentViewModel(Person person)
    {
        _person = person;
        this.Id = person.Id.ToString();
    }
    
    public PersonDocumentViewModel()
    {
        _person = new Person();
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    public void SaveEvent()
    {

        if (_person != null && _person.Id == 0 && !string.IsNullOrEmpty(_person.FirstName))
        {

            _person.CreationDate = DateTime.Now;
            var rtnValue = DataAccess.Local.PersonRepository.Insert(_person);
            Log.Logger.Information("Person: " + _person.FullNameLastFirst + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = DataAccess.Local.PersonRepository.Update(_person);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        //if (Browser != null) Browser.RefreshData();

    }
}