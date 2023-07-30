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

    public BrowserViewModel Browser = null;

    public PlantingDocumentViewModel(Planting planting, BrowserViewModel browser) : this(browser)
    {
        _planting = planting;
        this.Id = planting.Id.ToString();
        
        if (Planting.Plant.Id != 0 && _plants.Count > 0) Planting.Plant = Plants.First(x => x.Id == Planting.Plant.Id);
        if (Planting.SeedPacket.Id != 0 && SeedPackets.Count > 0) Planting.SeedPacket = SeedPackets.First(x => x.Id == Planting.SeedPacket.Id);
        if (Planting.Author.Id != 0 && People.Count > 0) Planting.Author = People.First(x => x.Id == Planting.Author.Id);
        if (Planting.State.Id != 0 && PlantingStates.Count > 0) Planting.State = PlantingStates.First(x => x.Id == Planting.State.Id);
    }

    public PlantingDocumentViewModel(BrowserViewModel browser)
    {
        Planting = new Planting();
        Browser = browser;

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
}
