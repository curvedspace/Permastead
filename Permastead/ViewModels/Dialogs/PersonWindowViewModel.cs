using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class PersonWindowViewModel : ViewModelBase
{
    public ObservableCollection<TagData> Items { get; set; }
    public ObservableCollection<TagData> SelectedItems { get; set; }
    public AutoCompleteFilterPredicate<object> FilterPredicate { get; set; }
    
    [ObservableProperty] 
    private Person _person;

    [ObservableProperty] private string _newTag;
    
    private ContactsViewModel _controlViewModel { get; set;  } = new ContactsViewModel();
    
    public PersonWindowViewModel()
    {
        _person = new Person();
        
        SelectedItems = new ObservableCollection<TagData>();
        Items = new ObservableCollection<TagData>(Services.PersonService.GetAllTags(AppSession.ServiceMode));
            
        FilterPredicate = Search;
    }

    public PersonWindowViewModel(Person person, ContactsViewModel obsVm) : this()
    {
        _person = person;
        _controlViewModel = obsVm;
        
        foreach (var tagData in _person.TagList)
        {
            var td = new TagData
            {
                TagText = tagData
            };
            SelectedItems.Add(td);
        }

    }
    
    private static bool Search(string? text, object? data)
    {
        if (text is null) return true;
        
        if (data is not TagData control) return false;
        return control.TagText.Contains(text, StringComparison.InvariantCultureIgnoreCase);
    }
    
    public void SaveRecord()
    {
        //check for a new tag to be added in the search text
        if (NewTag != "")
        {
            // if there is a new tag to be added, add it to selected items before saving
            var newTagModel = new TagData() { TagText =  NewTag }; 
            SelectedItems.Add(newTagModel);
        }
        
        Person.TagList.Clear();
        foreach (var tagData in SelectedItems)
        {
            Person.TagList.Add(tagData.TagText);
        }
        Person.SyncTags();
        
        var rtnValue = Services.PersonService.CommitRecord(AppSession.ServiceMode, Person);
        
        OnPropertyChanged(nameof(Person));
            
        using (LogContext.PushProperty("PersonViewModel", this))
        {
            Log.Information("Saved person: " + _person.FullNameLastFirst, rtnValue);
        }
        Items = new ObservableCollection<TagData>(Services.PersonService.GetAllTags(AppSession.ServiceMode));
        _controlViewModel.RefreshDataOnly();
        
    }
}