using Dock.Model.Mvvm.Controls;
using DynamicData.Binding;
using Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Permastead.ViewModels.Views;
using CommunityToolkit.Mvvm.Input;
using Services;

namespace Permastead.ViewModels.Tools;

public partial class Tool1ViewModel : Tool
{
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();

    [ObservableProperty]
    private Plant _currentItem;

    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }

    public Tool1ViewModel() 
    {
        _currentItem = new Plant();

        _plants = new ObservableCollection<Plant>(Services.PlantService.GetAllPlants(AppSession.ServiceMode));
    }

    [RelayCommand]
    public void Open()
    {
        if (_currentItem != null)
            this.Dock.OpenDoc(_currentItem);
    }

}
