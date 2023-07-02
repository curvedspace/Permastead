using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

    public partial class SeedPacketViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<SeedPacket> _packets;

        [ObservableProperty]
        private ObservableCollection<Vendor> _vendors;

        [ObservableProperty]
        private ObservableCollection<Plant> _plants;

        [ObservableProperty]
        private ObservableCollection<Person> _people;

        [ObservableProperty]
        private long _seedPacketsCount;

        [ObservableProperty]
        private SeedPacket _currentItem;

        [ObservableProperty]
        private bool _plantedOnly = false;
        
        [ObservableProperty]
        private bool _unplantedOnly = false;

        public SeedPacketViewModel()
        {
            try
            {
                _vendors = new ObservableCollection<Vendor>();
                _plants = new ObservableCollection<Plant>();
                _people = new ObservableCollection<Person>();
                _packets = new ObservableCollection<SeedPacket>();

                _currentItem = new SeedPacket();

                _vendors = new ObservableCollection<Vendor>(Services.VendorService.GetAll(AppSession.ServiceMode));
                _plants = new ObservableCollection<Plant>(Services.PlantService.GetAllPlants(AppSession.ServiceMode));
                _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));

                RefreshView();

                if (_packets.Count > 0)
                    CurrentItem = _packets.FirstOrDefault();
                else
                {
                    CurrentItem = new SeedPacket();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private void RefreshView()
        {
            _packets.Clear();
            var p = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, true);

            foreach (var o in p)
            {
                o.Author = _people.First(x => x.Id == o.Author.Id);
                o.Vendor = _vendors.First(x => x.Id == o.Vendor.Id);
                o.Plant = _plants.First(x => x.Id == o.Plant.Id);

                if (!PlantedOnly && !UnplantedOnly)
                {
                    _packets.Add(o);
                }
                else
                {
                    if (PlantedOnly)
                    {
                        if (o.IsPlanted == true)
                        {
                            _packets.Add(o);
                        }
                    }
                    else if (UnplantedOnly)
                    {
                        if (o.IsPlanted == false)
                        {
                            _packets.Add(o);
                        }
                    }
                }
            }

            if (_packets.Count > 0)
                _currentItem = _packets.FirstOrDefault();
            else
                _currentItem = new SeedPacket();

            SeedPacketsCount = _packets.Count;

        }

        [RelayCommand]
        private void SaveRecord()
        {
            //if there is a comment, save it.

            if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
            {

                CurrentItem.CreationDate = DateTime.Now;

                var rtnValue = DataAccess.Local.SeedPacketRepository.Insert(CurrentItem);

                if (rtnValue)
                {
                    _packets.Add(CurrentItem);
                }

                Console.WriteLine("saved " + rtnValue);

                RefreshView();
            }
            else
            {
                var rtnValue = DataAccess.Local.SeedPacketRepository.Update(CurrentItem);
                RefreshView();
            }

        }

        [RelayCommand]
        private void ResetRecord()
        {
            RefreshView();

            // reset the current item
            CurrentItem = new SeedPacket();
            OnPropertyChanged(nameof(CurrentItem));

        }
        
    }
