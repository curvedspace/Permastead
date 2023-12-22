using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantingWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private Planting _planting;
 
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();

    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty]
    private ObservableCollection<PlantingState> _plantingStates = new ObservableCollection<PlantingState>();

    [ObservableProperty]
    private ObservableCollection<PlantingObservation> _plantingObservations = new ObservableCollection<PlantingObservation>();
    
    public PlantingWindowViewModel(Planting planting) 
    {
        _planting = planting;
    }
}