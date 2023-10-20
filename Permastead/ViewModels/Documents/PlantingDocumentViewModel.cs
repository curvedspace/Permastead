using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using Dock.Model.Mvvm.Controls;

using Models;
using Permastead.ViewModels.Tools;

using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace Permastead.ViewModels.Documents;

public partial class PlantingDocumentViewModel : Document
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

    [ObservableProperty] private PlantingObservation _currentObservation = new PlantingObservation();
    
    public GreenhouseToolViewModel GreenhouseTool = null;

    public PlantingDocumentViewModel(Planting planting, GreenhouseToolViewModel greenhouseTool) : this(greenhouseTool)
    {
        _planting = planting;
        this.Id = planting.Id.ToString();
        
        if (Planting.Plant.Id != 0 && _plants.Count > 0) Planting.Plant = Plants.First(x => x.Id == Planting.Plant.Id);
        if (Planting.SeedPacket.Id != 0 && SeedPackets.Count > 0) Planting.SeedPacket = SeedPackets.First(x => x.Id == Planting.SeedPacket.Id);
        if (Planting.Author.Id != 0 && People.Count > 0) Planting.Author = People.First(x => x.Id == Planting.Author.Id);
        if (Planting.State.Id != 0 && PlantingStates.Count > 0) Planting.State = PlantingStates.First(x => x.Id == Planting.State.Id);
        
        PlantingObservations =
            new ObservableCollection<PlantingObservation>(
                Services.PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, Planting.Id));
    }

    public PlantingDocumentViewModel(GreenhouseToolViewModel greenhouseTool)
    {
        Planting = new Planting();
        GreenhouseTool = greenhouseTool;

        Plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        SeedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode));
        Beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        People = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        PlantingStates = new ObservableCollection<PlantingState>(Services.PlantingsService.GetPlantingStates(AppSession.ServiceMode));
        
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.

        if (_planting != null && _planting.Id == 0 && !string.IsNullOrEmpty(_planting.Description))
        {

            _planting.CreationDate = DateTime.Now;
 
            var rtnValue = DataAccess.Local.PlantingsRepository.Insert(_planting);

            Console.WriteLine("saved " + rtnValue);

        }
        else
        {
            var rtnValue = DataAccess.Local.PlantingsRepository.Update(_planting);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        //if (Browser != null) Browser.RefreshData();

    }
    
    [RelayCommand]
    private void Harvest()
    {
        Planting.EndDate = DateTime.Today;
        SaveEvent();
    }

    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author.Id = 2;
            CurrentObservation.Planting = Planting;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType.Id = 2;

            DataAccess.Local.PlantingsRepository.InsertPlantingObservation(DataAccess.DataConnection.GetLocalDataSource(),
                CurrentObservation);
        
            PlantingObservations =
                new ObservableCollection<PlantingObservation>(
                    Services.PlantingsService.GetObservationsForPlanting(AppSession.ServiceMode, Planting.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
    }
}
