using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;

namespace Permastead.ViewModels.Views;
public partial class InventoryViewModel: ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Inventory> _inventory = new ObservableCollection<Inventory>();

    [ObservableProperty] 
    private long _inventoryCount;
    
    [ObservableProperty] 
    private ObservableCollection<InventoryGroup> _inventoryGroups;
    
    [ObservableProperty] 
    private ObservableCollection<InventoryType> _inventoryTypes;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private Inventory _currentItem;
    
    [ObservableProperty] 
    private bool _forSaleOnly = false;

    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveToDo()
    {
        //if there is a record, save it.
        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Notes))
        {

            CurrentItem.CreationDate = DateTime.Now;

            var rtnValue = DataAccess.Local.InventoryRepository.Insert(CurrentItem);

            if (rtnValue)
            {
                _inventory.Add(CurrentItem);
            }

            Console.WriteLine("saved " + rtnValue);

            RefreshInventory();
        }
        else
        {
            var rtnValue = DataAccess.Local.InventoryRepository.Update(_currentItem);
            RefreshInventory();
        }

    }

    [RelayCommand]
    private void ResetToDo()
    {
        RefreshInventory();
        
        // reset the current item
        CurrentItem = new Inventory();
        OnPropertyChanged(nameof(CurrentItem));
        
    }
    
    public InventoryViewModel()
    {
        try
        {
            //get code tables
            _inventoryGroups = new ObservableCollection<InventoryGroup>(Services.InventoryService.GetAllInventoryGroups(AppSession.ServiceMode));
            _inventoryTypes = new ObservableCollection<InventoryType>(Services.InventoryService.GetAllInventoryTypes(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
            _currentItem = new Inventory();
            
            RefreshInventory();
            
            if (_inventory.Count > 0) CurrentItem = _inventory.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory.");
        }
    }
    
    [RelayCommand]
    private void RefreshInventory()
    {
        _inventory.Clear();

        var invList = Services.InventoryService.GetAllInventory(AppSession.ServiceMode);

        foreach (var inv in invList)
        {
            inv.InventoryGroup = _inventoryGroups.First(x => x.Id == inv.InventoryGroup.Id);
            inv.InventoryType = _inventoryTypes.First(x => x.Id == inv.InventoryType.Id);
            inv.Author = _people.First(x => x.Id == inv.Author.Id);

            if (_forSaleOnly)
            {
                if (inv.ForSale) _inventory.Add(inv);
            }
            else
            {
                _inventory.Add(inv);
            }
            
        }
        
        InventoryCount = _inventory.Count;
      
    }
}