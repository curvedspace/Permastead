using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryTypeWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private InventoryType _type;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
        
    private InventoryViewModel _controlViewModel { get; set;  } = new InventoryViewModel();
    
    public InventoryTypeWindowViewModel(InventoryType type, InventoryViewModel controlViewModel) : this(controlViewModel)
    {
        _type = type;
    }

    public InventoryTypeWindowViewModel(InventoryViewModel controlViewModel) : this()
    {
        Type = new InventoryType();
        _controlViewModel = controlViewModel;
    }
    
    public InventoryTypeWindowViewModel()
    {
        try
        {
            //get code tables
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory type.");
        }
    }
    
    public void SaveRecord()
    {
        //if there is a comment, save it.

        if (_type != null && _type.Id == 0 && !string.IsNullOrEmpty(_type.Description))
        {

            _type.CreationDate = DateTime.Now;
            
            var rtnValue = InventoryTypeService.CommitRecord(AppSession.ServiceMode, _type);
            Log.Logger.Information("Inventory Type: " + _type.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = InventoryTypeService.CommitRecord(AppSession.ServiceMode, _type);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        if (_controlViewModel != null) _controlViewModel.RefreshData();

    }
}