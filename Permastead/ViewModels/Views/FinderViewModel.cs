using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class FinderViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _searchText = "";
    
    [ObservableProperty]
    private ObservableCollection<SearchResult> _results = new ObservableCollection<SearchResult>();
    
    [ObservableProperty] 
    private bool _currentOnly = true;

    [ObservableProperty]
    private long _resultsCount;

    [ObservableProperty]
    private SearchResult _currentItem;

    public FlatTreeDataGridSource<SearchResult> SearchResultsSource { get; set; }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        Results.Clear();
        ResultsCount = Results.Count;
    }
    
    [RelayCommand]
    private void PerformSearch()
    {
        RefreshDataOnly(SearchText);
    }

    public void RefreshDataOnly(string textValue)
    {
        Results.Clear();
        
        var tempResults = new List<SearchResult>();
        
        tempResults = FinderService.FindRecords(AppSession.ServiceMode, textValue);

        foreach (var result in tempResults)
        {
            Results.Add(result);
        }

        
        SearchResultsSource = new FlatTreeDataGridSource<SearchResult>(Results)
        {
            Columns =
            {
                new TextColumn<SearchResult, string>
                    ("As Of Date", x => x.AsOfDateDateString),
                new TextColumn<SearchResult, string>
                    ("Entity Name", x => x.Entity.Name),
                new TextColumn<SearchResult, string>
                    ("Sub Type", x => x.SubType),
                new TextColumn<SearchResult, string>
                    ("Field", x => x.FieldName),
                new TextColumn<SearchResult, string>
                    ("Text Found", x => x.SearchText)
            },
        };
        
        if (Results.Count > 0) 
            CurrentItem = Results.FirstOrDefault()!;
        else
            CurrentItem = new SearchResult();

        ResultsCount = Results.Count;
    }
    
    public FinderViewModel()
    {
        RefreshDataOnly("");
    }
}