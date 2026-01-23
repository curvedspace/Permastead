using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class AnimalWindowViewModel : ViewModelBase
{
    public ObservableCollection<TagData> Items { get; set; }
    public ObservableCollection<TagData> SelectedItems { get; set; }
    public AutoCompleteFilterPredicate<object> FilterPredicate { get; set; }
    
    [ObservableProperty] 
    private Animal _currentItem;
    
    [ObservableProperty]
    private ObservableCollection<AnimalType> _animalTypes;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] private string _newTag;
    
    public AnimalsViewModel ControlViewModel { get; set;  } = new AnimalsViewModel();
    
    public void SaveRecord()
    {
        bool rtnValue;
        
        //check for a new tag to be added in the search text
        if (NewTag != "")
        {
            // if there is a new tag to be added, add it to selected items before saving
            var newTagModel = new TagData() { TagText =  NewTag }; 
            SelectedItems.Add(newTagModel);
        }

        CurrentItem.TagList.Clear();
        foreach (var tagData in SelectedItems)
        {
            CurrentItem.TagList.Add(tagData.TagText);
        }
        CurrentItem.SyncTags();
        
        rtnValue = Services.AnimalService.CommitRecord(AppSession.ServiceMode, CurrentItem);
        
        OnPropertyChanged(nameof(_currentItem));
            
        using (LogContext.PushProperty("AnimalWindowViewModel", this))
        {
            Log.Information("Saved animal: " + CurrentItem.Name, rtnValue);
        }
        
        ControlViewModel.RefreshDataOnly();
        Items = new ObservableCollection<TagData>(Services.AnimalService.GetAllTags(AppSession.ServiceMode));
        
    }
    
    private static bool Search(string? text, object? data)
    {
        if (text is null) return true;
        
        if (data is not TagData control) return false;
        return control.TagText.Contains(text, StringComparison.InvariantCultureIgnoreCase);
    }
    
    public AnimalWindowViewModel()
    {
        _animalTypes = new ObservableCollection<AnimalType>();
        _people = new ObservableCollection<Person>();
            
        _currentItem = new Animal();

        _animalTypes = new ObservableCollection<AnimalType>(AnimalService.GetAllAnimalTypes(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
        
        SelectedItems = new ObservableCollection<TagData>();
        Items = new ObservableCollection<TagData>(Services.AnimalService.GetAllTags(AppSession.ServiceMode));
            
        FilterPredicate = Search;
    }
    
    public AnimalWindowViewModel(Animal item, AnimalsViewModel obsVm) : this()
    {
        CurrentItem = item;
        ControlViewModel = obsVm;
        
        CurrentItem = item;
        
        if (CurrentItem != null)
        {
            if (CurrentItem.AnimalTypeId > 0) CurrentItem.Type = AnimalTypes.First(x => x.Id == CurrentItem.AnimalTypeId);
            CurrentItem.Author = People.First(x => x.Id == CurrentItem.Author.Id);
            
            foreach (var tagData in CurrentItem.TagList)
            {
                var td = new TagData
                {
                    TagText = tagData
                };
                SelectedItems.Add(td);
            }
        }
        
    }
}