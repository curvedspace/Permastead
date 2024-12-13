using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace Permastead.ViewModels.Dialogs;

public partial class HarvestWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Harvest _currentItem;
    
    [ObservableProperty] 
    private ObservableCollection<Entity> _entities;
    
    [ObservableProperty] 
    private ObservableCollection<MeasurementUnit> _measurementUnits;

    public HarvestWindowViewModel()
    {
        _entities = new ObservableCollection<Entity>();
        _entities.Add( new Entity {Id = 0, Name = "Unknown" });
    }
}