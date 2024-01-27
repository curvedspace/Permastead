
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    
    [ObservableProperty] private string _country;
    
    [ObservableProperty] private string _nostrPublicKey;
    
    [ObservableProperty] private string _nostrPrivateKey;

    [ObservableProperty] private ObservableCollection<City>? _cities;
    
    public string HomesteadNameText => _homesteadName.ToString();
    
    public string DatabaseLocationText => _databaseLocation.ToString();

    private Dictionary<string, string> _initialSettings;
    
    #region Constructors

    public SettingsViewModel()
    {
        //set a default db location
        DatabaseLocation = Services.SettingsService.GetLocalDatabaseSource();

        _homesteadName = "My Homestead";
        _firstName = "Homesteader";
        _lastName = "Person";
        _location = "";
        _country = "";
        _nostrPrivateKey = "";
        _nostrPublicKey = "";
        
        
        _initialSettings = SettingsService.GetAllSettings();

        try
        {
            HomesteadName = _initialSettings["HNAME"];
            FirstName = _initialSettings["FNAME"];
            LastName = _initialSettings["LNAME"];
            Location = _initialSettings["LOC"];
            Country = _initialSettings["CTRY"];
            NostrPublicKey = _initialSettings["NOSTRPUB"];
            NostrPrivateKey = _initialSettings["NOSTRPRIV"];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        //_cities = new ObservableCollection<City>(settingsService.GetCities().Values);
        //Console.WriteLine("cities has been loaded");
        
    }
    
    #endregion

    [RelayCommand]
    private void SaveSettings()
    {
        //save the settings to database
        bool rtnValue;
        
        //get existing values, only update the ones that have changed
        try
        {
            if (_initialSettings["FNAME"] != FirstName) rtnValue = SettingsRepository.Update("FNAME", FirstName);
            if (_initialSettings["LNAME"] != LastName) rtnValue = SettingsRepository.Update("LNAME", LastName);
            if (_initialSettings["HNAME"] != HomesteadName) rtnValue = SettingsRepository.Update("HNAME", HomesteadName);
            if (_initialSettings["LOC"] != Location) rtnValue = SettingsRepository.Update("LOC", Location);
            if (_initialSettings["CTRY"] != Country) rtnValue = SettingsRepository.Update("CTRY", Country);
            if (_initialSettings["NOSTRPUB"] != NostrPublicKey) rtnValue = SettingsRepository.Update("NOSTRPUB", NostrPublicKey);
            if (_initialSettings["NOSTRPRIV"] != NostrPrivateKey) rtnValue = SettingsRepository.Update("NOSTRPRIV", NostrPrivateKey);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    [RelayCommand]
    private void MigrateDbToServer()
    {
        Services.DbMigrationService.MigrateLocalToServer(DataAccess.DataConnection.GetLocalDataSource(), DataAccess.DataConnection.GetServerDataSource());
    }


}