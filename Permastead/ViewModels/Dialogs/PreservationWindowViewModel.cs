using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class PreservationWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private FoodPreservation _currentItem;
    
    [ObservableProperty] 
    private ObservableCollection<Harvest> _harvests;
    
    [ObservableProperty] 
    private ObservableCollection<MeasurementUnit> _measurementUnits;
    
    [ObservableProperty] 
    private ObservableCollection<FoodPreservationType> _preservationTypes;

    public PreservationViewModel ControlViewModel { get; set;  } = new PreservationViewModel();
    
    public void SaveRecord()
    {
        bool rtnValue;

        rtnValue = Services.FoodPreservationService.CommitRecord(AppSession.ServiceMode, CurrentItem);
        
        OnPropertyChanged(nameof(_currentItem));
            
        using (LogContext.PushProperty("PreservationWindowViewModel", this))
        {
            Log.Information("Saved preservation item: " + CurrentItem.Name, rtnValue);
        }
        
        ControlViewModel.RefreshDataOnly();
        
    }
    
    public PreservationWindowViewModel()
    {
        _preservationTypes = new ObservableCollection<FoodPreservationType>(Services.FoodPreservationService.GetAllPreservationTypes(AppSession.ServiceMode));
        _measurementUnits = new ObservableCollection<MeasurementUnit>(Services.MeasurementsService.GetAllMeasurementTypes(AppSession.ServiceMode));
        _harvests = new ObservableCollection<Harvest>(Services.HarvestService.GetAllHarvests(AppSession.ServiceMode));
        
        _harvests.Add(new Harvest() {Id = 0, Description = "Unavailable"});
    }

    public PreservationWindowViewModel(FoodPreservation preservation, PreservationViewModel preservationViewModel) : this()
    {
        CurrentItem = preservation;
        ControlViewModel = preservationViewModel;
        
        
        // now we have to reset the harvest so the databinding works (not sure why this is necessary)
        if (Harvests.Count > 0)
            CurrentItem.Harvest = Harvests.First( x => x.Id == CurrentItem.Harvest.Id);
    }
    
    
}