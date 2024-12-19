using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryWindowViewModel: ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<InventoryGroup> _inventoryGroups;
    
    [ObservableProperty] 
    private ObservableCollection<InventoryType> _inventoryTypes;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private Inventory _currentItem;
    
    public InventoryViewModel ControlViewModel { get; set;  } = new InventoryViewModel();

    public InventoryWindowViewModel()
    {
        try
        {
            //get code tables
            _inventoryGroups = new ObservableCollection<InventoryGroup>(Services.InventoryGroupService.GetAllInventoryGroups(AppSession.ServiceMode));
            _inventoryTypes = new ObservableCollection<InventoryType>(Services.InventoryTypeService.GetAllInventoryTypes(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
            _currentItem = new Inventory();
            
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
        
        _currentItem.InventoryGroup = _inventoryGroups.First(x => x.Id == inventory.InventoryGroup.Id);
        _currentItem.InventoryType = _inventoryTypes.First(x => x.Id == inventory.InventoryType.Id);
        _currentItem.Author = _people.First(x => x.Id == inventory.Author.Id);

    }
    
    public void SaveRecord()
    {
        bool rtnValue;

        rtnValue = Services.InventoryService.CommitRecord(AppSession.ServiceMode, _currentItem);
        
        OnPropertyChanged(nameof(_currentItem));
            
        using (LogContext.PushProperty("PersonViewModel", this))
        {
            Log.Information("Saved inventory item: " + _currentItem.Description, rtnValue);
        }
        
        ControlViewModel.RefreshData();
        
    }
    
}