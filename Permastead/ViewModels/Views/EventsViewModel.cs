using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
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
    
    public FlatTreeDataGridSource<AnEvent> EventsSource { get; set; }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshEvents();
    }

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
    
    public void RefreshEvents()
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
        var centered = new TextColumnOptions<AnEvent>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        EventsSource = new FlatTreeDataGridSource<AnEvent>(Events)
        {
            Columns =
            {
                new TextColumn<AnEvent, DateTime>
                    ("Next Date", x => x.NextDate),
                new TextColumn<AnEvent, string>
                    ("Type", x => x.AnEventType.Description),
                new TextColumn<AnEvent, string>
                    ("Description", x => x.Description),
                new TextColumn<AnEvent, string>
                    ("Assigner", x => x.Assigner.FirstName),
                new TextColumn<AnEvent, string>
                    ("Assignee", x => x.Assignee.FirstName),
                new TextColumn<AnEvent, string>
                    ("Frequency", x => x.Frequency.Description),
                new TextColumn<AnEvent, long>
                    ("Warning Days", x => x.WarningDays),
                new CheckBoxColumn<AnEvent>
                (
                    "ToDo Trigger",
                    x => x.ToDoTrigger,
                    (o, v) => o.ToDoTrigger = v,
                    options: new()
                    {
                        CanUserResizeColumn = false, CanUserSortColumn = true
                    }),
                
                new TextColumn<AnEvent, DateTime>
                    ("Last Triggered", x => x.LastTriggerDate),
                new TextColumn<AnEvent, DateTime>
                    ("Last Updated", x => x.LastUpdatedDate)
            },
        };
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
            
            bool rtnValue;

            if (AppSession.ServiceMode == ServiceMode.Local)
            {
                rtnValue = DataAccess.Local.AnEventRepository.Insert(CurrentItem);
            }
            else
            {
                rtnValue = DataAccess.Server.AnEventRepository.Insert(CurrentItem);
            }
            
            
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