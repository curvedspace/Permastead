using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class VendorWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private Vendor _vendor;
    
    private PlantingsViewModel _controlViewModel { get; set;  } = new PlantingsViewModel();

    public VendorWindowViewModel(Vendor vendor, PlantingsViewModel controlViewModel) : this(controlViewModel)
    {
        _vendor = vendor;
    }

    public VendorWindowViewModel(PlantingsViewModel controlViewModel)
    {
        Vendor = new Vendor();
        _controlViewModel = controlViewModel;
    }
    
    
    public void SaveRecord()
    {
        //if there is a comment, save it.

        if (_vendor != null && _vendor.Id == 0 && !string.IsNullOrEmpty(_vendor.Description))
        {

            _vendor.CreationDate = DateTime.Now;
            var rtnValue = VendorService.CommitRecord(AppSession.ServiceMode, _vendor);
            Log.Logger.Information("Vendor: " + _vendor.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = VendorService.CommitRecord(AppSession.ServiceMode, _vendor);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        if (_controlViewModel != null) _controlViewModel.RefreshData();

    }
    
}