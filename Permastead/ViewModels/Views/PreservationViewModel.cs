using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Services;

namespace Permastead.ViewModels.Views;

public partial class PreservationViewModel : ViewModelBase
{
    [ObservableProperty] 
    private bool _currentOnly = true;

    [ObservableProperty]
    private long _preservationCount;

    [ObservableProperty]
    private FoodPreservation _currentItem;
    
    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty]
    private string _searchText = "";
    
    [ObservableProperty]
    private ObservableCollection<FoodPreservation> _foodPreservations = new ObservableCollection<FoodPreservation>();
    
    [ObservableProperty] private FoodPreservationObservation _currentObservation = new FoodPreservationObservation();
    
    [ObservableProperty]
    private ObservableCollection<FoodPreservationObservation> _preservationObservations = new ObservableCollection<FoodPreservationObservation>();
    
    public FlatTreeDataGridSource<FoodPreservation> PreservationSource { get; set; }
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }

    public void GetPreservationObservations()
    {
        if (CurrentItem != null)
        {
            PreservationObservations =
                new ObservableCollection<FoodPreservationObservation>(
                    Services.FoodPreservationService.GetObservationsForPreservation(AppSession.ServiceMode, CurrentItem.Id));
        }
    }
    
    public void RefreshDataOnly(string filterText = "")
    {
        FoodPreservations.Clear();

        var list = Services.FoodPreservationService.GetAll(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();
        
        foreach (var item in list)
        {
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                if (CurrentOnly)
                {
                    if (item.IsActive()) FoodPreservations.Add(item);
                }
                else
                {
                    FoodPreservations.Add(item);
                }
                
            }
            else
            {
                if (item.Name.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    item.PreservationType.Description!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    item.Harvest.Description!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    item.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    FoodPreservations.Add(item);
                }
            }
        }
        
        var centered = new TextColumnOptions<FoodPreservation>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        PreservationSource = new FlatTreeDataGridSource<FoodPreservation>(FoodPreservations)
        {
            Columns =
            {
                new TextColumn<FoodPreservation, string>
                    ("Start Date", x => x.StartDateString),
                new TextColumn<FoodPreservation, string>
                    ("Name", x => x.Name),
                new TextColumn<FoodPreservation, string>
                    ("Harvest", x => x.Harvest.Description),
                new TextColumn<FoodPreservation, string>
                    ("Harvest Type", x => x.PreservationType!.Description),
                new TextColumn<FoodPreservation, long>
                    ("Measurement", x => x.Measurement, GridLength.Auto,centered),
                new TextColumn<FoodPreservation, string>
                    ("Units", x => x.Units.Description),
                new TextColumn<FoodPreservation, string>
                    ("Author", x => x.Author!.FullName()),
                new TextColumn<FoodPreservation, string>
                    ("Comment", x => x.Comment)
            },
        };
        
        
        if (FoodPreservations.Count > 0) 
            CurrentItem = FoodPreservations.FirstOrDefault()!;
        else
            CurrentItem = new FoodPreservation();

        PreservationCount = FoodPreservations.Count;
    }
    
    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author = AppSession.Instance.CurrentUser;
            CurrentObservation.FoodPreservation = CurrentItem;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 2;
            
            FoodPreservationService.AddPreservationObservation(AppSession.ServiceMode, CurrentObservation);
            
            PreservationObservations =
                 new ObservableCollection<FoodPreservationObservation>(Services.FoodPreservationService.GetObservationsForPreservation(AppSession.ServiceMode, CurrentItem.Id));

            CurrentObservation = new FoodPreservationObservation();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    public void EditRecord()
    {
        // open the selected row in a window for viewing/editing
        var preservationWindow = new PreservationWindow();
        
        //get the selected row in the list
        var current = CurrentItem;
        if (current != null)
        {
            var vm = new PreservationWindowViewModel(current, this);

            preservationWindow.DataContext = vm;

            preservationWindow.Topmost = true;
            preservationWindow.Width = 800;
            preservationWindow.Height = 500;
            preservationWindow.Opacity = 0.95;
            preservationWindow.Title = "Preservation - " + current.Name;
            preservationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            preservationWindow.Show();
        }
    }
    
    [RelayCommand]
    public void AddRecord()
    {
        // open the selected row in a window for viewing/editing
        var preservationWindow = new PreservationWindow();
        
        //get the selected row in the list
        var current = new FoodPreservation();
        current.Author = AppSession.Instance.CurrentUser;
        current.StartDate = DateTime.Today;
        current.Measurement = 1;
        current.EndDate = DateTime.MaxValue;

        
        if (current != null)
        {
            var vm = new PreservationWindowViewModel(current, this);

            preservationWindow.DataContext = vm;

            preservationWindow.Topmost = true;
            preservationWindow.Width = 800;
            preservationWindow.Height = 500;
            preservationWindow.Opacity = 0.95;
            preservationWindow.Title = "New Preservation";
            preservationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            preservationWindow.Show();
        }
    }

    [RelayCommand]
    public void CreateInventoryRecord()
    {
        // open the selected planting in a window for viewing/editing
        var inventoryWindow = new InventoryWindow();
        
        //get the selected row in the list
        var current = CurrentItem;
        
        if (current != null)
        {
            var vm = new InventoryWindowViewModel();

            vm.CurrentItem.Description = CurrentItem.Name;
            vm.CurrentItem.Brand = "Homemade";
            vm.CurrentItem.Quantity = 1;
            vm.CurrentItem.InventoryGroup = "Food";
            vm.CurrentItem.InventoryType = "Preserved";
            vm.CurrentItem.Room = "Kitchen";
            vm.CurrentItem.Author = vm.People.First(x => x.Id == AppSession.Instance.CurrentUser.Id);

            vm.PreservationItem = current;

            inventoryWindow.DataContext = vm;

            inventoryWindow.Topmost = true;
            inventoryWindow.Width = 1000;
            inventoryWindow.Height = 650;
            inventoryWindow.Opacity = 0.95;
            inventoryWindow.Title = "Inventory - " + current.Name;
            inventoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inventoryWindow.Show();
        }
    }
    
    public PreservationViewModel()
    {
        RefreshDataOnly();
    }

}