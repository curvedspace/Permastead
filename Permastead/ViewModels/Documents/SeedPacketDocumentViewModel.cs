using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Controls;
using Models;
using Permastead.ViewModels.Tools;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;


namespace Permastead.ViewModels.Documents;

public partial class SeedPacketDocumentViewModel : Document
{
    
    [ObservableProperty]
    private SeedPacket _seedPacket;

    [ObservableProperty]
    private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();

    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors;

    public Tool5ViewModel PropertyViewModel = new Tool5ViewModel();

    public SeedPacketDocumentViewModel(SeedPacket seedPacket) : this()
    {
        _seedPacket = seedPacket;
        this.Id = seedPacket.Id.ToString();

        if (_seedPacket.Plant.Id != 0 && _plants.Count > 0) _seedPacket.Plant = _plants.First(x => x.Id == _seedPacket.Plant.Id);
        if (_seedPacket.Author.Id != 0 && _people.Count > 0) _seedPacket.Author = _people.First(x => x.Id == _seedPacket.Author.Id);
        if (_seedPacket.Vendor.Id != 0 && _vendors.Count > 0) _seedPacket.Vendor = _vendors.First(x => x.Id == _seedPacket.VendorId);
    }

    public SeedPacketDocumentViewModel()
    {
        _seedPacket = new SeedPacket();

        _plants = new ObservableCollection<Plant>(Services.PlantingsService.GetPlants(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
        _vendors = new ObservableCollection<Vendor>(Services.VendorService.GetAll(AppSession.ServiceMode));
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
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
}
