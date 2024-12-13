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
        RefreshDataOnly(SearchText);
    }
    
     public void RefreshDataOnly(string filterText = "")
    {
        Harvests.Clear();
        MeasurementUnits.Clear();

        var harvests = new List<Harvest>(); //Services.AnimalService.GetAllAnimals(AppSession.ServiceMode);
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
                    harvest.Type.Description!.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
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
                    ("Date", x => x.HarvestDateString),
                new TextColumn<Harvest, string>
                    ("Description", x => x.Description),
                new TextColumn<Harvest, string>
                    ("Units", x => x.Units.ToString()),
                new TextColumn<Harvest, long>
                    ("Measurement", x => x.Measurement),
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
    
}