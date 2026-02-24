
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls.Selection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using DataAccess.Local;
using Models;
using Services;
using Ursa.Controls;

namespace Permastead.ViewModels.Views;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _homesteadName;
    
    [ObservableProperty]
    private string _databaseLocation;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private Person _currentUser;

    [ObservableProperty] private string _firstName;
    
    [ObservableProperty] private string _lastName;
    
    [ObservableProperty] private string _location;
    
    [ObservableProperty] private string _country;
    
    [ObservableProperty] private string _nostrPublicKey;
    
    [ObservableProperty] private string _nostrPrivateKey;

    [ObservableProperty] private string _propertyImage;
    
    [ObservableProperty]
    private Bitmap _propertyPicture; 
    
    public WindowToastManager? ToastManager { get; set; }

    public bool InServerMode
    {
        get
        {
            return (AppSession.ServiceMode == ServiceMode.Server && !string.IsNullOrEmpty(DataAccess.DataConnection.GetServerConnectionString()));
        }
    }
    
    public bool InLocalMode
    {
        get
        {
            return !InServerMode;
        }
    }
    
    public bool ServerDbExists
    {
        get
        {
            return (!string.IsNullOrEmpty(DataAccess.DataConnection.GetServerConnectionString()));
        }
    }

    public bool ShowMigrateDataButton
    {
        get
        {
            return (InLocalMode && !string.IsNullOrEmpty(DataAccess.DataConnection.GetServerConnectionString()));
        }
    }

    [ObservableProperty] private ObservableCollection<City>? _cities;
    
    public string HomesteadNameText => _homesteadName.ToString();
    
    public string DatabaseLocationText => _databaseLocation.ToString();

    private Dictionary<string, string> _initialSettings;
    
    #region Constructors

    public SettingsViewModel()
    {
        //set a default db location
        DatabaseLocation = Services.SettingsService.GetLocalDatabaseSource();
        var currentUserId = DataConnection.GetCurrentUserId();
        
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllOnsitePeople(AppSession.ServiceMode));
        CurrentUser = PersonService.GetPersonFromId(AppSession.ServiceMode, currentUserId);
        
        _homesteadName = "My Homestead";
        
        
        if (_currentUser != null)
        {
            FirstName = _currentUser.FirstName;
            LastName = _currentUser.LastName;
            
            if (_people.Count > 0)
                CurrentUser = _people.First(x => x.Id == _currentUser.Id);
            
            AppSession.Instance.CurrentUser = CurrentUser;
        }
        else
        {
            _firstName = "Homesteader";
            _lastName = "Person";
        }
        
            
        _location = "";
        _country = "";
        _nostrPrivateKey = "";
        _nostrPublicKey = "";
        
        
        _initialSettings = SettingsService.GetAllSettings(AppSession.ServiceMode);
       

        try
        {
            HomesteadName = _initialSettings["HNAME"];
            //FirstName = _initialSettings["FNAME"];
            //LastName = _initialSettings["LNAME"];
            Location = _initialSettings["LOC"];
            Country = _initialSettings["CTRY"];
            NostrPublicKey = _initialSettings["NOSTRPUB"];
            NostrPrivateKey = _initialSettings["NOSTRPRIV"];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        var newLocation = "/Assets/permaculture_plan.jpg";
        
        AssetLoader.GetAssets(new Uri("avares://Permastead"),null);
        
        
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
        if (isWindows)
        {
            newLocation = userFolder + @"\.config\permastead\images\properties\" + HomesteadName + ".png";
            
        }
        else
        {
            newLocation = userFolder + @"/.config/permastead/images/properties/" + HomesteadName + ".png";
            
        }

        
        // Open reading stream from the first file.
        FileInfo nfi = new FileInfo(newLocation);

        if (nfi.Exists)
        {

            using var stream = nfi.OpenRead();
            {
                Bitmap pic = new Bitmap(stream);

                if (pic != null)
                {
                    PropertyPicture = pic;
                    ToastManager?.Show(new Toast("Image has been added for " + HomesteadName +
                                                    "."));
                }
            }
        }
        else
        {
            PropertyPicture = new Bitmap(AssetLoader.Open(new Uri("avares://Permastead/Assets/permaculture_plan.jpg"), null));
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
            // if (_initialSettings["FNAME"] != FirstName) rtnValue = SettingsService.UpdateSetting("FNAME", FirstName, AppSession.ServiceMode);
            // if (_initialSettings["LNAME"] != LastName) rtnValue = SettingsService.UpdateSetting("LNAME", LastName, AppSession.ServiceMode);
            if (_initialSettings["HNAME"] != HomesteadName) rtnValue = SettingsService.UpdateSetting("HNAME", HomesteadName, AppSession.ServiceMode);
            if (_initialSettings["LOC"] != Location) rtnValue = SettingsService.UpdateSetting("LOC", Location, AppSession.ServiceMode);
            if (_initialSettings["CTRY"] != Country) rtnValue = SettingsService.UpdateSetting("CTRY", Country, AppSession.ServiceMode);
            if (_initialSettings["NOSTRPUB"] != NostrPublicKey) rtnValue = SettingsService.UpdateSetting("NOSTRPUB", NostrPublicKey, AppSession.ServiceMode);
            if (_initialSettings["NOSTRPRIV"] != NostrPrivateKey) rtnValue = SettingsService.UpdateSetting("NOSTRPRIV", NostrPrivateKey, AppSession.ServiceMode);

            DataConnection.SetCurrentUserId(CurrentUser.Id);
            
            FirstName = CurrentUser.FirstName;
            LastName = CurrentUser.LastName;
            CurrentUser = People.First(x => x.Id == CurrentUser.Id);
            
            AppSession.Instance.CurrentUser = CurrentUser;
            
            //save out database location
            var dbLocation = DatabaseLocationText;

            if (AppSession.ServiceMode == ServiceMode.Local)
            {
                DataConnection.SetDefaultDatabaseLocation(dbLocation);
            }
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

    [RelayCommand]
    private void MigrateDbToLocal()
    {
        Services.DbMigrationService.MigrateServerToLocal(DataAccess.DataConnection.GetLocalDataSource(), DataAccess.DataConnection.GetServerDataSource());
    }

}