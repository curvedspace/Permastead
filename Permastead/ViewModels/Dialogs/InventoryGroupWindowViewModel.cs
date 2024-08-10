using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryGroupWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private InventoryGroup _group;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    private InventoryViewModel _controlViewModel { get; set;  } = new InventoryViewModel();
    
    public InventoryGroupWindowViewModel(InventoryGroup group, InventoryViewModel controlViewModel) : this(controlViewModel)
    {
        _group = group;
    }

    public InventoryGroupWindowViewModel(InventoryViewModel controlViewModel) : this()
    {
        Group = new InventoryGroup();
        _controlViewModel = controlViewModel;
    }
    
    public InventoryGroupWindowViewModel()
    {
        try
        {
            //get code tables
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting inventory group.");
        }
    }
    
    
    public void SaveRecord()
    {
        //if there is a comment, save it.

        if (_group != null && _group.Id == 0 && !string.IsNullOrEmpty(_group.Description))
        {

            _group.CreationDate = DateTime.Now;
            
            var rtnValue = InventoryGroupService.CommitRecord(AppSession.ServiceMode, _group);
            Log.Logger.Information("Inventory Group: " + _group.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = InventoryGroupService.CommitRecord(AppSession.ServiceMode, _group);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        if (_controlViewModel != null) _controlViewModel.RefreshData();

    }
}
