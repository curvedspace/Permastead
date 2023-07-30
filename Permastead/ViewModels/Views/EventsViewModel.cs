using System;
using System.Collections.ObjectModel;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class  EventsViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<AnEvent> _events;

    [ObservableProperty] 
    private ObservableCollection<AnEventType> _eventTypes;
    
    [ObservableProperty]
    private ObservableCollection<Frequency> _frequencies;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;

    [ObservableProperty] 
    private long _activeToDos;

    [ObservableProperty] 
    private long _eventsCount;
    
    [ObservableProperty] 
    private AnEvent _currentItem;
    
    public EventsViewModel()
    {
        try
        {
            _eventTypes = new ObservableCollection<AnEventType>();
            _frequencies = new ObservableCollection<Frequency>();
            _people = new ObservableCollection<Person>();
            _events = new ObservableCollection<AnEvent>();
            
            CurrentItem = new AnEvent();

            _eventTypes = new ObservableCollection<AnEventType>(Services.EventsService.GetAllEventTypes(AppSession.ServiceMode));
            _frequencies = new ObservableCollection<Frequency>(Services.FrequencyService.GetAll(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            
            RefreshEvents();
            
            if (_events.Count > 0) 
                CurrentItem = _events.FirstOrDefault();
            else
            {
                CurrentItem = new AnEvent();
            }

            
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    private void RefreshEvents()
    {
        Events.Clear();

        var _myEvents = Services.EventsService.GetAllEvents(AppSession.ServiceMode);

        foreach (var e in _myEvents)
        {
            e.Frequency = Frequencies.First(x => x.Id == e.Frequency.Id);
            e.AnEventType = EventTypes.First(x => x.Id == e.AnEventType.Id);
            e.Assignee = People.First(x => x.Id == e.Assignee.Id);
            e.Assigner = People.First(x => x.Id == e.Assigner.Id);
            
            Events.Add(e);
            
            EventsCount = Events.Count;
        }
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.

        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {

            CurrentItem.CreationDate = DateTime.Now;
            CurrentItem.LastTriggerDate = CurrentItem.StartDate;
            
            var rtnValue = DataAccess.Local.AnEventRepository.Insert(CurrentItem);
            
            if (rtnValue)
            {
                Events.Add(CurrentItem);
            }
            
            Console.WriteLine("saved " + rtnValue);

            RefreshEvents();
        }
        else
        {
            var rtnValue = DataAccess.Local.AnEventRepository.Update(CurrentItem);
            RefreshEvents();
        }

    }

    [RelayCommand]
    private void ResetEvent()
    {
        RefreshEvents();
        
        // reset the current item
        CurrentItem = new AnEvent();
        OnPropertyChanged(nameof(CurrentItem));
        
    }

}