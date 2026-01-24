using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Serilog;
using Serilog.Context;
using Services;
using Ursa.Controls;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryWindowViewModel: ViewModelBase
{
    public ObservableCollection<TagData> Items { get; set; }
    public ObservableCollection<TagData> SelectedItems { get; set; }
    public AutoCompleteFilterPredicate<object> FilterPredicate { get; set; }
    
    [ObservableProperty] private string _newTag;
    
    [ObservableProperty] 
    private ObservableCollection<string> _inventoryGroups;
    
    [ObservableProperty] 
    private ObservableCollection<string> _inventoryTypes;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private ObservableCollection<string> _brands;
    
    [ObservableProperty] 
    private ObservableCollection<string> _rooms;
    
    [ObservableProperty] 
    private Inventory _currentItem;
    
    [ObservableProperty] 
    private FoodPreservation _preservationItem;
    
    public InventoryViewModel ControlViewModel { get; set;  } = new InventoryViewModel();

    public InventoryWindowViewModel()
    {
        try
        {
            //get code tables
            _inventoryGroups = new ObservableCollection<string>(Services.InventoryService.GetAllGroups(AppSession.ServiceMode));
            _inventoryTypes = new ObservableCollection<string>(Services.InventoryService.GetAllTypes(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            _brands = new ObservableCollection<string>(Services.InventoryService.GetAllBrands(AppSession.ServiceMode));
            _rooms = new ObservableCollection<string>(Services.InventoryService.GetAllRooms(AppSession.ServiceMode));
            
            _currentItem = new Inventory();
            
            SelectedItems = new ObservableCollection<TagData>();
            Items = new ObservableCollection<TagData>(Services.InventoryService.GetAllTags(AppSession.ServiceMode));
            
            FilterPredicate = Search;
            
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory.");
        }
    }
    
    public InventoryWindowViewModel(Inventory inventory, InventoryViewModel obsVm) : this()
    {
        _currentItem = inventory;
        ControlViewModel = obsVm;
        
        _currentItem.Author = _people.First(x => x.Id == inventory.Author.Id);
        
        foreach (var tagData in _currentItem.TagList)
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
        if (!string.IsNullOrEmpty(NewTag))
        {
            // if there is a new tag to be added, add it to selected items before saving
            var newTagModel = new TagData() { TagText =  NewTag }; 
            
            // check to see if the newtag is the search text from the last selected item - if so, do not add it again
            // only add if it a truly new tag
            var lastTag = SelectedItems.LastOrDefault();
            
            if (lastTag == null)
                SelectedItems.Add(newTagModel);
            else if  (!lastTag.TagText.Contains(newTagModel.TagText, StringComparison.InvariantCultureIgnoreCase))
                SelectedItems.Add(newTagModel);
        }
        
        CurrentItem.TagList.Clear();
        foreach (var tagData in SelectedItems)
        {
            CurrentItem.TagList.Add(tagData.TagText);
        }
        CurrentItem.SyncTags();
        
        var rtnValue = Services.InventoryService.CommitRecord(AppSession.ServiceMode, CurrentItem);

        OnPropertyChanged(nameof(CurrentItem));

        using (LogContext.PushProperty("PersonViewModel", this))
        {
            Log.Information("Saved inventory item: " + CurrentItem.Description, rtnValue);
        }

        ControlViewModel.RefreshData();
        ControlViewModel.ToastManager?.Show(new Toast("Inventory has been updated."));

        Items = new ObservableCollection<TagData>(Services.InventoryService.GetAllTags(AppSession.ServiceMode));
        
        if (PreservationItem != null)
        {
            //set its end date to today to indicate that it has been moved to inventory
            PreservationItem.EndDate = DateTime.Now;
            FoodPreservationService.CommitRecord(AppSession.ServiceMode, PreservationItem);
        }
    }
    
}