using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Common;
using Permastead.ViewModels.Dialogs;
using Services;

namespace Permastead.Views.Dialogs;

public partial class PlantingWizard : Window
{
    public PlantingWizard()
    {
        InitializeComponent();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantingWizardViewModel vm  = (PlantingWizardViewModel)DataContext;
        
        //check for a new vendor record - if yes, create it and assign ID
        var vendors = Services.VendorService.GetAll(AppSession.ServiceMode);
        
        bool createVendor = true;
        var currentVendorCtrl = this.FindControl<AutoCompleteBox>("VendorName");
        var currentVendor = "";
        
        vm.CurrentPlanting.Author.Id = AppSession.Instance.CurrentUser.Id;
        vm.CurrentPlanting.StartDate = DateTime.UtcNow;
        vm.CurrentPlanting.EndDate = DateTime.MaxValue;
        vm.CurrentPlanting.Description = vm.CurrentPlant.Description;

        if (currentVendorCtrl != null)
        {
            currentVendor = currentVendorCtrl.Text;

            foreach (var vendor in vendors)
            {
                if (vendor.Description == currentVendor)
                {
                    createVendor = false;
                    vm.CurrentPlanting.SeedPacket.Vendor = vendor;
                }
            }
        }

        // if needed, create the new vendor record and assign it to the current planting.
        if (createVendor)
        {
            var newVendor = new Vendor();
            newVendor.Description = currentVendor;
            newVendor.Code = currentVendor.ToUpperInvariant();
            Services.VendorService.CommitRecord(AppSession.ServiceMode, newVendor);
            
            vendors = Services.VendorService.GetAll(AppSession.ServiceMode);
            foreach (var vendor in vendors)
            {
                if (vendor.Description == currentVendor)
                {
                    vm.CurrentPlanting.SeedPacket.Vendor = vendor;
                }
            }
        }
        
        //now look at seed packet, if it does not exist we should create it on the fly
        if (vm.CurrentPlanting.SeedPacket.Id == 0)
        {
            vm.CurrentPlanting.Plant = vm.CurrentPlant;
            vm.CurrentPlanting.SeedPacket.Code = vm.CurrentPlant.Code;
            vm.CurrentPlanting.SeedPacket.Description = vm.CurrentPlant.Description;
            vm.CurrentPlanting.SeedPacket.StartDate = DateTime.UtcNow;
            vm.CurrentPlanting.SeedPacket.EndDate = DateTime.MaxValue;
            vm.CurrentPlanting.SeedPacket.Author.Id = AppSession.Instance.CurrentUser.Id;
            vm.CurrentPlanting.SeedPacket.Plant.Id = vm.CurrentPlant.Id;
            vm.CurrentPlanting.Comment = vm.CurrentPlanting.SeedPacket.Instructions;
            
            //if the starter is not a seed packet, assume that it needs to be ended today
            if (vm.CurrentPlanting.SeedPacket.StarterType.Code != "SEED")
            {
                vm.CurrentPlanting.SeedPacket.EndDate = DateTime.UtcNow;
            }
            
            Services.SeedPacketService.CommitRecord(AppSession.ServiceMode, vm.CurrentPlanting.SeedPacket);
            
            //now retrieve the new record and assign it
            var seedPackets = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode);
            foreach (var seedPacket in seedPackets)
            {
                if (seedPacket.Description == vm.CurrentPlanting.SeedPacket.Description)
                {
                    vm.CurrentPlanting.SeedPacket.Id = seedPacket.Id;
                }
            }
        }
        
        //finally check for location
        bool createLocation = true;
        var beds = Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode);
        var currentLocationCtrl = this.FindControl<AutoCompleteBox>("LocationName");
        var currentLocation = "";

        if (currentLocationCtrl != null)
        {
            currentLocation = currentLocationCtrl.Text;
            foreach (var bed in beds)
            {
                if (bed.Description == currentLocation)
                {
                    vm.CurrentPlanting.Bed = bed;
                    createLocation =  false;
                }
            }
            
            // if needed, create and assign the new location aka garden bed
            if (createLocation)
            {
                var newLocation = new GardenBed();
                newLocation.Description = currentLocation;
                newLocation.Code = TextUtils.Codify(currentLocation,10);
                
                if (AppSession.ServiceMode == ServiceMode.Local)
                {
                    DataAccess.Local.GardenBedRepository.Insert(newLocation);
                }
                else
                {
                    DataAccess.Server.GardenBedRepository.Insert(newLocation);
                }
            
                beds = Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode);
                foreach (var bed in beds)
                {
                    if (bed.Description == currentLocation)
                    {
                        vm.CurrentPlanting.Bed = bed;
                    }
                }
            }
        }
        
        //finally commit the planting record
        Services.PlantingsService.CommitRecord(AppSession.ServiceMode, vm.CurrentPlanting);
        
        this.Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}