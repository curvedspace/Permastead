using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.ViewModels.Dialogs;

public partial class StarterWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private SeedPacket _seedPacket;

    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();

    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors;
    
    [ObservableProperty]
    private ObservableCollection<Seasonality> _seasonalities;
    
    [ObservableProperty]
    private ObservableCollection<SeedPacketObservation> _seedPacketObservations = new ObservableCollection<SeedPacketObservation>();
    
    [ObservableProperty] private SeedPacketObservation _currentObservation = new SeedPacketObservation();

    private PlantingsViewModel _controlViewModel { get; set;  } = new PlantingsViewModel();

    public StarterWindowViewModel(SeedPacket seedPacket) : this()
    {
        _seedPacket = seedPacket;
        
        if (_seedPacket.Plant.Id != 0 && _plants.Count > 0) _seedPacket.Plant = _plants.First(x => x.Id == _seedPacket.Plant.Id);
        if (_seedPacket.Author.Id != 0 && _people.Count > 0) _seedPacket.Author = _people.First(x => x.Id == _seedPacket.Author.Id);
        if (_seedPacket.Vendor.Id != 0 && _vendors.Count > 0) _seedPacket.Vendor = _vendors.First(x => x.Id == _seedPacket.VendorId);
        if (_seedPacket.Seasonality.Id != 0 && _seasonalities.Count > 0) _seedPacket.Seasonality = _seasonalities.First(x => x.Id == _seedPacket.Seasonality.Id);
        
        SeedPacketObservations =
            new ObservableCollection<SeedPacketObservation>(
                Services.PlantingsService.GetObservationsForSeedPacket(AppSession.ServiceMode, SeedPacket.Id));
    }

    public StarterWindowViewModel()
    {
        _seedPacket = new SeedPacket();

        _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        _vendors = new ObservableCollection<Vendor>(Services.VendorService.GetAll(AppSession.ServiceMode));
        _seasonalities = new ObservableCollection<Seasonality>(Services.PlantingsService.GetSeasonalities(AppSession.ServiceMode));
    }
    
    // The method that will be executed when the command is invoked
    public void SaveRecord()
    {
        //if there is a comment, save it.

        if (_seedPacket != null && _seedPacket.Id == 0 && !string.IsNullOrEmpty(_seedPacket.Description))
        {
            _seedPacket.CreationDate = DateTime.Now;
 
            var rtnValue = DataAccess.Local.SeedPacketRepository.Insert(_seedPacket);

            Console.WriteLine("saved " + rtnValue);
        }
        else
        {
            var rtnValue = DataAccess.Local.SeedPacketRepository.Update(_seedPacket);
            
        }

    }
    
    // public void SaveObservation()
    // {
    //     try
    //     {
    //         //saves the seed packet observation to database
    //         CurrentObservation.Author.Id = 2;
    //         CurrentObservation.SeedPacket = SeedPacket;
    //         CurrentObservation.AsOfDate = DateTime.Today;
    //         CurrentObservation.CommentType.Id = 2;
    //
    //         DataAccess.Local.SeedPacketRepository.InsertSeedPacketObservation(DataAccess.DataConnection.GetLocalDataSource(),
    //             CurrentObservation);
    //     
    //         SeedPacketObservations =
    //             new ObservableCollection<SeedPacketObservation>(
    //                 Services.PlantingsService.GetObservationsForSeedPacket(AppSession.ServiceMode, SeedPacket.Id));
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         
    //     }
    //     
    // }
}