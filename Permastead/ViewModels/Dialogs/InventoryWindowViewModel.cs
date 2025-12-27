using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Serilog;
using Serilog.Context;
using Ursa.Controls;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryWindowViewModel: ViewModelBase
{
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
        ControlViewModel.ToastManager?.Show(new Toast("Inventory has been updated."));
        
    }
    
}