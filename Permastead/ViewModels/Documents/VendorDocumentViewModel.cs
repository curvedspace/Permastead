using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using Dock.Model.Mvvm.Controls;

using Models;
using Permastead.ViewModels.Tools;

using System;

using Serilog;

namespace Permastead.ViewModels.Documents;

public partial class VendorDocumentViewModel : Document
{
 
    [ObservableProperty]
    private Vendor _vendor;
    
    public GreenhouseToolViewModel GreenhouseTool = null;

    public VendorDocumentViewModel(Vendor vendor, GreenhouseToolViewModel greenhouseTool) : this(greenhouseTool)
    {
        _vendor = vendor;
        this.Id = vendor.Id.ToString();
    }

    public VendorDocumentViewModel(GreenhouseToolViewModel greenhouseTool)
    {
        Vendor = new Vendor();
        GreenhouseTool = greenhouseTool;
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.

        if (_vendor != null && _vendor.Id == 0 && !string.IsNullOrEmpty(_vendor.Description))
        {

            _vendor.CreationDate = DateTime.Now;
            var rtnValue = DataAccess.Local.VendorRepository.Insert(_vendor);
            Log.Logger.Information("Vendor: " + _vendor.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = DataAccess.Local.VendorRepository.Update(_vendor);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        if (GreenhouseTool != null) GreenhouseTool.RefreshData();

    }
    
    [RelayCommand]
    private void Harvest()
    {
        Vendor.EndDate = DateTime.Today;
        SaveEvent();
    }
}
