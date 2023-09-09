
using System;
using System.Collections.ObjectModel;
using AIMLbot.AIMLTagHandlers;
using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess;
using DataAccess.Local;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _homesteadName;
    
    [ObservableProperty]
    private string _databaseLocation;

    [ObservableProperty] private string _firstName;
    
    [ObservableProperty] private string _lastName;
    
    [ObservableProperty] private string _location;

    [ObservableProperty] private ObservableCollection<City> _cities;
    
    public string HomesteadNameText => _homesteadName.ToString();
    
    public string DatabaseLocationText => _databaseLocation.ToString();
    
    #region Constructors

    public SettingsViewModel()
    {
        //set a default db location
        this._databaseLocation = Services.SettingsService.GetLocalDatabaseSource();

        _homesteadName = "My Homestead";
        _firstName = "Homesteader";
        _lastName = "Person";
        _location = "";

        
        //var gaia = new Services.GaiaService();
        
        var settings = SettingsService.GetAllSettings();

        try
        {
            HomesteadName = settings["HNAME"];
            FirstName = settings["FNAME"];
            LastName = settings["LNAME"];
            Location = settings["LOC"];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        //_cities = new ObservableCollection<City>(settingsService.GetCities().Values);
        //Console.WriteLine("cities has been loaded");
        
    }
    
    #endregion
}