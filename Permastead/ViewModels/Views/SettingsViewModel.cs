using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
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

    [ObservableProperty] private ObservableCollection<City> _cities;
    
    public string HomesteadNameText => _homesteadName.ToString();
    
    public string DatabaseLocationText => _databaseLocation.ToString();
    
    #region Constructors

    public SettingsViewModel()
    {
        //set a default db location
        this._databaseLocation = Services.SettingsService.GetLocalDatabaseSource();

        this._homesteadName = "My Homestead";

        _firstName = "Homesteader";

        _lastName = "Person";

        //var gaia = new Services.GaiaService();

        var settingsService = new SettingsService();

        //_cities = new ObservableCollection<City>(settingsService.GetCities().Values);
        
        Console.WriteLine("cities has been loaded");

    }
    
    #endregion
}