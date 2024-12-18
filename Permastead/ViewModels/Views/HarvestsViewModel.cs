using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

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
    
    [RelayCommand]
    private void RefreshData()
    {
        //_measurementUnits = Services.HarvestService.GetAllHarvests()
            
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
                    ("Measurement", x => x.Measurement),
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
    
    public HarvestsViewModel()
    {
        RefreshDataOnly();
    }
    
}