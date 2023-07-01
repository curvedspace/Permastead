using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;

namespace Permastead.ViewModels.Views;

public partial class PlantingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty]
    private ObservableCollection<GardenBed> _beds = new ObservableCollection<GardenBed>();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
    
    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;

    [ObservableProperty] 
    private long _plantingCount;
    
    [ObservableProperty] 
    private bool _currentOnly = false;
    
    [ObservableProperty] 
    private Planting _currentItem;
    
    public PlantingsViewModel()
    {
        try
        {
            _beds = new ObservableCollection<GardenBed>(Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode));
            _seedPackets = new ObservableCollection<SeedPacket>(Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, false));
            _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));

            _currentItem = new Planting();

            RefreshPlantings();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting plantings.");
        }
            
    }
    
    [RelayCommand]
    private void RefreshPlantings()
    {
        _plantings.Clear();
        var p = Services.PlantingsService.GetPlantings(AppSession.ServiceMode);

        foreach (var o in p)
        {
            o.SeedPacket = _seedPackets.First(x => x.Id == o.SeedPacket.Id);
            o.Author = _people.First(x => x.Id == o.Author.Id);
            o.Bed = _beds.First(x => x.Id == o.Bed.Id);
            o.Plant = _plants.First(x => x.Id == o.Plant.Id);

            if (_currentOnly)
            {
                if (o.IsActive) _plantings.Add(o);
            }
            else
            {
                _plantings.Add(o);
            }
            
        }

        if (_plantings.Count > 0) 
            _currentItem = _plantings.FirstOrDefault();
        else
            _currentItem = new Planting();

        PlantingCount = _plantings.Count;

    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.

        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {

            CurrentItem.CreationDate = DateTime.Now;
 
            var rtnValue = DataAccess.Local.PlantingsRepository.Insert(CurrentItem);
            
            if (rtnValue)
            {
                _plantings.Add(CurrentItem);
            }
            
            Console.WriteLine("saved " + rtnValue);

            RefreshPlantings();
        }
        else
        {
            var rtnValue = DataAccess.Local.PlantingsRepository.Update(CurrentItem);
            RefreshPlantings();
        }

    }

    [RelayCommand]
    private void ResetEvent()
    {
        RefreshPlantings();
        
        // reset the current item
        CurrentItem = new Planting();
        OnPropertyChanged(nameof(CurrentItem));
        
    }
}