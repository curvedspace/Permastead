using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace Permastead.ViewModels.Dialogs;

public partial class PlantWindowViewModel : ViewModelBase
{
    
    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] 
    private Plant _plant;
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();

    
}