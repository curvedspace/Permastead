using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using Models;
using Permastead.ViewModels.Tools;
using Serilog;

namespace Permastead.ViewModels.Documents;

public partial class PlantDocumentViewModel : Document
{
    [ObservableProperty] 
    private long _plantingCount;

    [ObservableProperty] 
    private Plant _plant;


    public PlantDocumentViewModel(Plant plant)
    {
        _plant = plant;
        this.Id = plant.Id.ToString();
        this.PlantingCount = 10;
    }
    
    public PlantDocumentViewModel()
    {
        _plant = new Plant();
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    public void SaveEvent()
    {

        if (_plant != null && _plant.Id == 0 && !string.IsNullOrEmpty(_plant.Description))
        {

            _plant.CreationDate = DateTime.Now;
            var rtnValue = DataAccess.Local.PlantRepository.Insert(_plant);
            Log.Logger.Information("Plant: " + _plant.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = DataAccess.Local.PlantRepository.Update(_plant);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        //if (Browser != null) Browser.RefreshData();

    }
}