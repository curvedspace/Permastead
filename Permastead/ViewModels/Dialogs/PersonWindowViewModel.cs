using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class PersonWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Person _person;
    
    private ContactsViewModel _controlViewModel { get; set;  } = new ContactsViewModel();
    
    public PersonWindowViewModel()
    {
        _person = new Person();
    }

    public PersonWindowViewModel(Person person, ContactsViewModel obsVm) : this()
    {
        _person = person;
        _controlViewModel = obsVm;

    }
    
    public void SaveRecord()
    {
        bool rtnValue;

        rtnValue = Services.PersonService.CommitRecord(AppSession.ServiceMode, _person);
        
        OnPropertyChanged(nameof(_person));
            
        using (LogContext.PushProperty("PersonViewModel", this))
        {
            Log.Information("Saved person: " + _person.FullNameLastFirst, rtnValue);
        }
        
        _controlViewModel.RefreshDataOnly();
        
    }
}