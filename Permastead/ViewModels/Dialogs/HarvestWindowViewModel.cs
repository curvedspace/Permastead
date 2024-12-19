using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        
        _measurementUnits = new ObservableCollection<MeasurementUnit>(Services.MeasurementsService.GetAllMeasurementTypes(AppSession.ServiceMode));
        _harvestTypes = new ObservableCollection<HarvestType>(Services.HarvestService.GetAllHarvestTypes(AppSession.ServiceMode));
        
        OtherList = new List<Entity>();
        OtherList.Add( new Entity { Id = 0, Name = "Not Available" });
        
        AnimalsList = Services.AnimalService.GetAnimalsAsEntityList(AppSession.ServiceMode);
        PlantingsList = Services.PlantingsService.GetPlantingsAsEntityList(AppSession.ServiceMode);
        
        this.SetEntityList();
    }
    
    public HarvestWindowViewModel(Harvest item, HarvestsViewModel obsVm) : this()
    {
        CurrentItem = item;
        _controlViewModel = obsVm;
        this.SetEntityList();
        
        CurrentItem = item;
        
        // now we have to reset the harvest entity so the databinding works (not sure why this is necessary)
        CurrentItem.HarvestEntity = Entities.First( x => x.Id == item.HarvestEntity.Id);
    }

    public void SetEntityList()
    {
        if (CurrentItem != null && CurrentItem.HarvestType != null)
        {
            switch (CurrentItem.HarvestType.Description)
            {
                case "Animal":
                    Entities = new ObservableCollection<Entity>(AnimalsList);
                    break;
                
                case "Plant":
                    Entities = new ObservableCollection<Entity>(PlantingsList);
                    break;
                
                default:
                    Entities = new ObservableCollection<Entity>(OtherList);
                    break;
            }
        }
        else
        {
            Entities = new ObservableCollection<Entity>(OtherList);
        }
    }
}