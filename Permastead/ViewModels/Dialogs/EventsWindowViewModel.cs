using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class EventsWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<AnEventType> _eventTypes;
    
    [ObservableProperty]
    private ObservableCollection<Frequency> _frequencies;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    [ObservableProperty] 
    private AnEvent _currentItem;
    
    public EventsViewModel ControlViewModel { get; set;  } = new EventsViewModel();
    
    public EventsWindowViewModel()
    {
        try
        {
            //get code tables
            _eventTypes = new ObservableCollection<AnEventType>(Services.EventsService.GetAllEventTypes(AppSession.ServiceMode));
            _frequencies = new ObservableCollection<Frequency>(Services.FrequencyService.GetAll(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));
            
            _currentItem = new AnEvent();
            
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Error getting events.");
        }
    }
    
    public EventsWindowViewModel(AnEvent e, EventsViewModel obsVm) : this()
    {
        CurrentItem = e;
        ControlViewModel = obsVm;
        
        CurrentItem.AnEventType = EventTypes.First(x => x.Id == e.AnEventType.Id);
        CurrentItem.Frequency = Frequencies.First(x => x.Id == e.Frequency.Id);
        CurrentItem.Assignee = People.First(x => x.Id == e.Assignee.Id);
        CurrentItem.Assigner = People.First(x => x.Id == e.Assigner.Id);

    }
    
    public void SaveRecord()
    {
        bool rtnValue;

        rtnValue = Services.EventsService.CommitRecord(AppSession.ServiceMode, CurrentItem);
        
        OnPropertyChanged(nameof(CurrentItem));
            
        using (LogContext.PushProperty("EventsViewModel", this))
        {
            Log.Information("Saved events item: " + _currentItem.Description, rtnValue);
        }
        
        ControlViewModel.RefreshEvents();
        
    }
}