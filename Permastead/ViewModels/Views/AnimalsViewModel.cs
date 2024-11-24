using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace Permastead.ViewModels.Views;

public partial class AnimalsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Animal> _animals;

    [ObservableProperty]
    private ObservableCollection<AnimalType> _animalTypes;

    [ObservableProperty]
    private long _animalCount;

    [ObservableProperty]
    private Animal _currentItem;
    
    [ObservableProperty]
    private string _searchText = "";
    
    [RelayCommand]
    private void RefreshData()
    {


    }
}