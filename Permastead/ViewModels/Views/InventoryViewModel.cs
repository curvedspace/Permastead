using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;
using Services;

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
    
    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty] private InventoryObservation _currentObservation = new InventoryObservation();
    
    [ObservableProperty]
    private ObservableCollection<InventoryObservation> _inventoryObservations = new ObservableCollection<InventoryObservation>();
    
    public FlatTreeDataGridSource<Inventory> InventorySource { get; set; }

    public void GetInventoryObservations()
    {
        if (CurrentItem != null)
        {
            InventoryObservations = new ObservableCollection<InventoryObservation>(Services.InventoryService.GetObservationsForInventoryItem(AppSession.ServiceMode, CurrentItem.Id));
        }
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveToDo()
    {
        //if there is a record, save it.
        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Notes))
        {

            CurrentItem.CreationDate = DateTime.Now;
            CurrentItem.Author = AppSession.Instance.CurrentUser;

            bool rtnValue;

            rtnValue = InventoryService.CommitRecord(AppSession.ServiceMode, CurrentItem);
            

            if (rtnValue)
            {
                _inventory.Add(CurrentItem);
            }

            Console.WriteLine("saved " + rtnValue);

            RefreshInventory();
        }
        else
        {
            var rtnValue = InventoryService.CommitRecord(AppSession.ServiceMode, CurrentItem);
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
            _inventoryGroups = new ObservableCollection<InventoryGroup>(Services.InventoryGroupService.GetAllInventoryGroups(AppSession.ServiceMode));
            _inventoryTypes = new ObservableCollection<InventoryType>(Services.InventoryTypeService.GetAllInventoryTypes(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
            _currentItem = new Inventory();
            
            RefreshInventory();
            
            if (_inventory.Count > 0) CurrentItem = _inventory.FirstOrDefault();
            
            GetInventoryObservations();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory.");
        }
    }
    
    [RelayCommand]
    private void RefreshInventory()
    {
        this.RefreshData();
    }
    
    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author!.Id = AppSession.Instance.CurrentUser.Id;
            CurrentObservation.Inventory = CurrentItem;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 2;
            
            InventoryService.AddInventoryObservation(AppSession.ServiceMode, CurrentObservation);
            
            InventoryObservations =
                new ObservableCollection<InventoryObservation>(
                    Services.InventoryService.GetObservationsForInventoryItem(AppSession.ServiceMode, CurrentItem.Id));

            CurrentObservation = new InventoryObservation();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    public void RefreshData()
    {
        _inventory.Clear();

        _inventoryGroups = new ObservableCollection<InventoryGroup>(Services.InventoryGroupService.GetAllInventoryGroups(AppSession.ServiceMode));
        _inventoryTypes = new ObservableCollection<InventoryType>(Services.InventoryTypeService.GetAllInventoryTypes(AppSession.ServiceMode));
        
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
        
        var centered = new TextColumnOptions<Inventory>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        InventorySource = new FlatTreeDataGridSource<Inventory>(_inventory)
        {
            Columns =
            {
                new TextColumn<Inventory, DateTime>
                    ("Date", x => x.CreationDate),
                new TextColumn<Inventory, string>
                    ("Brand", x => x.Brand),
                new TextColumn<Inventory, string>
                    ("Description", x => x.Description),
                new TextColumn<Inventory, string>
                    ("Author", x => x.Author!.FirstName),
                new TextColumn<Inventory, string>
                    ("Room", x => x.Room),
                new TextColumn<Inventory, string>
                    ("Group", x => x.InventoryGroup.Description),
                new TextColumn<Inventory, string>
                    ("Type", x => x.InventoryType.Description),
                new TextColumn<Inventory, long>
                    ("Quantity", x => x.Quantity,GridLength.Auto,centered),
                new TextColumn<Inventory, double>
                    ("Original Value", x => x.OriginalValue),
                new TextColumn<Inventory, double>
                    ("Current Value", x => x.CurrentValue),
                new TextColumn<Inventory, bool>
                    ("For Sale", x => x.ForSale),
                // new CheckBoxColumn<Inventory>
                // (
                //     "For Sale",
                //     x => x.ForSale,
                //     (o, v) => o.ForSale = v,
                //     options: new()
                //     {
                //         CanUserResizeColumn = false, CanUserSortColumn = true
                //     }),
                new TextColumn<Inventory, string>
                    ("Notes", x => x.Notes)
            },
        };
        
        InventoryCount = _inventory.Count;
    }
}