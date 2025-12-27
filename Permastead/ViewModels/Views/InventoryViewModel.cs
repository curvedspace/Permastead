using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Serilog;
using Services;
using Ursa.Controls;

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
    
    [ObservableProperty]
    private string _searchText = "";
    
    [ObservableProperty] private InventoryObservation _currentObservation = new InventoryObservation();
    
    [ObservableProperty]
    private ObservableCollection<InventoryObservation> _inventoryObservations = new ObservableCollection<InventoryObservation>();
    
    public FlatTreeDataGridSource<Inventory> InventorySource { get; set; }
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this inventory item?";
    private string _message;
    private string? _title = "Deletion Confirmation";
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }
    
    public ICommand YesNoCommand { get; set; }

    public WindowToastManager? ToastManager { get; set; }
    
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
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshData(SearchText);
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
            
            if (_inventory.Count > 0) CurrentItem = Inventory.FirstOrDefault();
            
            GetInventoryObservations();
            
            YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
            Icons = new ObservableCollection<MessageBoxIcon>(
                Enum.GetValues<MessageBoxIcon>());
            SelectedIcon = MessageBoxIcon.Question;
            _message = _shortMessage;

        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory.");
        }
    }
    
    [RelayCommand]
    private void RefreshInventory()
    {
        this.RefreshData(SearchText);
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

    [RelayCommand]
    private void EditInventory()
    {
        // open the selected planting in a window for viewing/editing
        var inventoryWindow = new InventoryWindow();

        var inventory = CurrentItem;
        if (inventory != null)
        {
            //get underlying view's viewmodel
            var vm = new InventoryWindowViewModel(inventory, this);
            
            inventoryWindow.DataContext = vm;
        
            inventoryWindow.Topmost = true;
            inventoryWindow.Width = 900;
            inventoryWindow.Height = 550;
            inventoryWindow.Opacity = 0.95;
            inventoryWindow.Title = "Inventory Item - " + inventory.Description;
        }

        inventoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        inventoryWindow.Show();
    }
    
    [RelayCommand]
    private async void DeleteInventory()
    {
        try
        {
            if (CurrentItem != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    //remove the record
                    InventoryService.DeleteRecord(AppSession.ServiceMode, CurrentItem);
                    RefreshInventory();
                    ToastManager?.Show(new Toast("Inventory record has been removed."));
                }
            }
        }
        catch (Exception e)
        {
        }
    }

    public void RefreshData(string filterText = "")
    {
        Inventory.Clear();

        InventoryGroups = new ObservableCollection<InventoryGroup>(Services.InventoryGroupService.GetAllInventoryGroups(AppSession.ServiceMode));
        InventoryTypes = new ObservableCollection<InventoryType>(Services.InventoryTypeService.GetAllInventoryTypes(AppSession.ServiceMode));
        
        var invList = Services.InventoryService.GetAllInventory(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();

        foreach (var inv in invList)
        {
            //inv.InventoryGroup = _inventoryGroups.First(x => x.Id == inv.InventoryGroup.Id);
            //inv.InventoryType = _inventoryTypes.First(x => x.Id == inv.InventoryType.Id);
            inv.Author = _people.First(x => x.Id == inv.Author.Id);
            
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                if (_forSaleOnly)
                {
                    if (inv.ForSale) _inventory.Add(inv);
                }
                else
                {
                    _inventory.Add(inv);
                }
            }
            else
            {
                if (inv.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    inv.InventoryGroup!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    inv.Notes.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    inv.Brand.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    inv.InventoryType.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    if (_forSaleOnly)
                    {
                        if (inv.ForSale) _inventory.Add(inv);
                    }
                    else
                    {
                        _inventory.Add(inv);
                    }
                }
            }
        }
        
        var centered = new TextColumnOptions<Inventory>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        var rightAligned = new TextColumnOptions<Inventory>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Right
        };
        
        InventorySource = new FlatTreeDataGridSource<Inventory>(Inventory)
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
                    ("Group", x => x.InventoryGroup),
                new TextColumn<Inventory, string>
                    ("Type", x => x.InventoryType),
                new TextColumn<Inventory, long>
                    ("Quantity", x => x.Quantity,GridLength.Auto,centered),
                new TextColumn<Inventory, double>
                    ("Original Value", x => x.OriginalValue,GridLength.Auto,rightAligned),
                new TextColumn<Inventory, double>
                    ("Current Value", x => x.CurrentValue,GridLength.Auto,rightAligned),
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
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task Show(MessageBoxButton button)
    {
        Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
    }
}