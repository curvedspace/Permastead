using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.ViewModels.Dialogs;

public partial class HarvestWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Harvest _currentItem;
    
    [ObservableProperty] 
    private ObservableCollection<Entity> _entities;
    
    [ObservableProperty] 
    private ObservableCollection<MeasurementUnit> _measurementUnits;
    
    [ObservableProperty] 
    private ObservableCollection<HarvestType> _harvestTypes;
    
    public List<Entity> PlantingsList;
    public List<Entity> AnimalsList;
    public List<Entity> OtherList;
    
    private HarvestsViewModel _controlViewModel { get; set;  } = new HarvestsViewModel();

    public HarvestWindowViewModel()
    {
        _entities = new ObservableCollection<Entity>();
        _entities.Add( new Entity {Id = 0, Name = "Unknown" });
        
        _currentItem = new Harvest();
        _currentItem.HarvestType!.Id = 1;
        
        _measurementUnits = new ObservableCollection<MeasurementUnit>(Services.MeasurementsService.GetAllMeasurementTypes(AppSession.ServiceMode));
        _harvestTypes = new ObservableCollection<HarvestType>(Services.HarvestService.GetAllHarvestTypes(AppSession.ServiceMode));
        
        OtherList = new List<Entity>();
        OtherList.Add( new Entity { Id = 0, Name = "Not Available" });
        
        AnimalsList = new List<Entity>();
        PlantingsList = new List<Entity>();
    }
    
    public HarvestWindowViewModel(Harvest item, HarvestsViewModel obsVm) : this()
    {
        _currentItem = item;
        _controlViewModel = obsVm;

    }
    
}