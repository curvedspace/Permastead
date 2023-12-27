using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;

namespace Permastead.ViewModels.Dialogs;

public partial class LocationWindowViewModel : ViewModelBase
{
    
    [ObservableProperty] 
    private GardenBed _bed;
    
    [ObservableProperty]
    private ObservableCollection<GardenBedType> _bedTypes = new ObservableCollection<GardenBedType>();


    public LocationWindowViewModel(GardenBed bed) : this()
    {
        _bed = bed;
        
        if (_bed.GardenBedTypeId != 0 && _bedTypes.Count > 0) _bed.Type = _bedTypes.First(x => x.Id == _bed.Type.Id);
    }
    
    public LocationWindowViewModel()
    {
        _bed = new GardenBed();
        
        _bedTypes = new ObservableCollection<GardenBedType>(Services.PlantingsService.GetGardenBedTypes(AppSession.ServiceMode));
    }

    // The method that will be executed when the command is invoked
    public void SaveRecord()
    {

        if (_bed != null && _bed.Id == 0 && !string.IsNullOrEmpty(_bed.Description))
        {
        
            _bed.CreationDate = DateTime.Now;
            var rtnValue = DataAccess.Local.GardenBedRepository.Insert(_bed);
            Log.Logger.Information("Planting Location: " + _bed.Description + " saved: " + rtnValue);
        
        }
        else
        {
            var rtnValue = DataAccess.Local.GardenBedRepository.Update(_bed);
            
        }

    }

}