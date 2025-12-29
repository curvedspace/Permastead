using System;
using System.Collections.Generic;
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
using Ursa.Controls;

namespace Permastead.ViewModels.Views;

public partial class HarvestsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Harvest> _harvests = new ObservableCollection<Harvest>();

    [ObservableProperty]
    private ObservableCollection<MeasurementUnit> _measurementUnits = new ObservableCollection<MeasurementUnit>();
    
    [ObservableProperty] 
    private bool _currentOnly = true;

    [ObservableProperty]
    private long _harvestsCount;

    [ObservableProperty]
    private Harvest _currentItem;
    
    [ObservableProperty]
    private string _searchText = "";
    
    public FlatTreeDataGridSource<Harvest> HarvestSource { get; set; }
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this harvest?";
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
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }
    
    public void RefreshDataOnly(string filterText = "")
    {
        Harvests.Clear();

        var harvests = Services.HarvestService.GetAllHarvests(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();
        
        foreach (var harvest in harvests)
        {
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                Harvests.Add(harvest);
            }
            else
            {
                if (harvest.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    harvest.HarvestType.Description!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    harvest.HarvestEntity.Name!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    harvest.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    Harvests.Add(harvest);
                }
            }
        }
        
        var centered = new TextColumnOptions<Harvest>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        HarvestSource = new FlatTreeDataGridSource<Harvest>(Harvests)
        {
            Columns =
            {
                new TextColumn<Harvest, string>
                    ("Harvest Date", x => x.HarvestDateString),
                new TextColumn<Harvest, string>
                    ("Description", x => x.Description),
                new TextColumn<Harvest, string>
                    ("Entity Name", x => x.HarvestEntity.Name),
                new TextColumn<Harvest, string>
                    ("Harvest Type", x => x.HarvestType!.Description),
                new TextColumn<Harvest, long>
                    ("Measurement", x => x.Measurement, GridLength.Auto, centered),
                new TextColumn<Harvest, string>
                    ("Units", x => x.Units.Description),
                new TextColumn<Harvest, string>
                    ("Author", x => x.Author!.FullName()),
                new TextColumn<Harvest, string>
                    ("Comment", x => x.Comment)
            },
        };
        
        if (Harvests.Count > 0) 
            CurrentItem = Harvests.FirstOrDefault()!;
        else
            CurrentItem = new Harvest();

        HarvestsCount = Harvests.Count;
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    private void EditHarvest()
    {
        // open the selected planting in a window for viewing/editing
        var harvestWindow = new HarvestWindow();
        
        
        if (CurrentItem != null)
        {
            //get underlying view's viewmodel
            var vm = new HarvestWindowViewModel(CurrentItem, this);
            
            harvestWindow.DataContext = vm;
        
            harvestWindow.Topmost = true;
            harvestWindow.Width = 1000;
            harvestWindow.Height = 550;
            harvestWindow.Opacity = 0.95;
            harvestWindow.Title = "Harvest - " + CurrentItem.Description;
        }

        harvestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        harvestWindow.Show();
        
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    private void PreserveHarvest()
    {
        // open the selected harvest in a preservation window for viewing/editing
        try
        {
            // open the selected planting in a harvest window 
            var preservationWindow = new PreservationWindow();
        
            //get the selected row in the list
            var current = CurrentItem;
        
            if (current != null)
            {
                var pres = new FoodPreservation();
                pres.Author = AppSession.Instance.CurrentUser;
                pres.Name = current.Description;
                pres.StartDate = DateTime.Today;
                
                var pvm = new PreservationViewModel();
                pvm.CurrentItem = pres;

                
                var vm = new PreservationWindowViewModel(pres, pvm);

                preservationWindow.DataContext = vm;

                preservationWindow.Topmost = true;
                preservationWindow.Width = 800;
                preservationWindow.Height = 500;
                preservationWindow.Opacity = 0.95;
                preservationWindow.Title = "Preservation - " + current.Description;
                preservationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                preservationWindow.Show();
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    [RelayCommand]
    private async void DeleteHarvest()
    {
        try
        {
            if (CurrentItem != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    //remove the record
                    Services.HarvestService.DeleteRecord(AppSession.ServiceMode, CurrentItem);
                    RefreshDataOnly();
                }
            }
        }
        catch (Exception e)
        {
        }
        
    }
    
    public HarvestsViewModel()
    {
        RefreshDataOnly();
        
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.Question;
        _message = _shortMessage;
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