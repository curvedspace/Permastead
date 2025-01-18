using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

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
    
    public HarvestsViewModel ControlViewModel { get; set;  } = new HarvestsViewModel();

    public HarvestWindowViewModel()
    {
        _entities = new ObservableCollection<Entity>();
        _entities.Add( new Entity {Id = 0, Name = "Unknown" });
        
        _currentItem = new Harvest();
        _currentItem.Author!.Id= 1;
        
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
        ControlViewModel = obsVm;
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
                    
                    if (CurrentItem.HarvestEntity != null)
                    {
                        CurrentItem.HarvestEntity.Id = 0;
                    }
                    else
                    {
                        CurrentItem.HarvestEntity = new Entity();
                        CurrentItem.HarvestEntity.Id = 0;
                    }
                    
                    Entities = new ObservableCollection<Entity>(OtherList);
                    
                    break;
            }
        }
        else
        {
            Entities = new ObservableCollection<Entity>(OtherList);
        }
    }
    
    public void SaveRecord()
    {
        bool rtnValue;

        rtnValue = Services.HarvestService.CommitRecord(AppSession.ServiceMode, CurrentItem);
        
        OnPropertyChanged(nameof(_currentItem));
            
        using (LogContext.PushProperty("HarvestWindowViewModel", this))
        {
            Log.Information("Saved inventory item: " + CurrentItem.Description, rtnValue);
        }
        
        ControlViewModel.RefreshDataOnly();
        
    }
}