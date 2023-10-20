using Dock.Model.Mvvm.Controls;
using DynamicData.Binding;
using Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Permastead.ViewModels.Views;
using CommunityToolkit.Mvvm.Input;
using Services;

namespace Permastead.ViewModels.Tools;

public partial class PeopleToolViewModel : Tool
{
    [ObservableProperty]
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();

    [ObservableProperty]
    private Person _currentItem;

    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }

    public PeopleToolViewModel() 
    {
        _currentItem = new Person();

        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
    }

    [RelayCommand]
    public void Open()
    {
        if (_currentItem != null)
            this.Dock.OpenDoc(_currentItem);
    }

}
