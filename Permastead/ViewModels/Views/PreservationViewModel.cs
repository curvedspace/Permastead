using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
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
    
    [ObservableProperty] private FoodPreservationObservation _currentObservation = new FoodPreservationObservation();
    
    [ObservableProperty]
    private ObservableCollection<FoodPreservationObservation> _preservationObservations = new ObservableCollection<FoodPreservationObservation>();

    
    public FlatTreeDataGridSource<FoodPreservation> PreservationSource { get; set; }
    
    [RelayCommand]
    private void RefreshData()
    {
        
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
            
            //AnimalService.AddAnimalObservation(AppSession.ServiceMode, CurrentObservation);
            
            // AnimalObservations =
            //     new ObservableCollection<AnimalObservation>(Services.AnimalService.GetObservationsForAnimal(AppSession.ServiceMode, CurrentItem.Id));

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
        //RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    public void EditRecord()
    {
        
    }
}