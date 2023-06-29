using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Controls;
using Models;
using Permastead.ViewModels.Tools;

namespace Permastead.ViewModels.Documents;

public partial class PlantDocumentViewModel : Document
{
    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] 
    private Plant _plant;

    public Tool5ViewModel PropertyViewModel = new Tool5ViewModel();

    public PlantDocumentViewModel(Plant plant)
    {
        _plant = plant;
        this.Id = plant.Id.ToString();
        this.PlantingCount = 10;
    }
    
    public PlantDocumentViewModel()
    {
        _plant = new Plant();
        PropertyViewModel = new Tool5ViewModel();
    }
}