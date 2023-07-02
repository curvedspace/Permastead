using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Controls;
using Models;
using Permastead.ViewModels.Tools;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using CommunityToolkit.Mvvm.Input;

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

    public BrowserViewModel Browser = null;

    public PlantingDocumentViewModel(Planting planting, BrowserViewModel browser) : this(browser)
    {
        _planting = planting;
        this.Id = planting.Id.ToString();
        
        if (_planting.Plant.Id != 0 && _plants.Count > 0) _planting.Plant = _plants.First(x => x.Id == _planting.Plant.Id);
        if (_planting.SeedPacket.Id != 0 && _seedPackets.Count > 0) _planting.SeedPacket = _seedPackets.First(x => x.Id == _planting.SeedPacket.Id);
        if (_planting.Author.Id != 0 && _people.Count > 0) _planting.Author = _people.First(x => x.Id == _planting.Author.Id);
    }

    public PlantingDocumentViewModel(BrowserViewModel browser)
    {
        _planting = new Planting();
        Browser = browser;

        _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode));
        _beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
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
        if (Browser != null) Browser.RefreshData();

    }
    
    [RelayCommand]
    private void Harvest()
    {
        Planting.EndDate = DateTime.Today;
        SaveEvent();
    }
}
