using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;

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
    
    public FlatTreeDataGridSource<FoodPreservation> PreservationSource { get; set; }
    
    [RelayCommand]
    private void RefreshData()
    {
        
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        //RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    public void EditRecord()
    {
        
    }
}