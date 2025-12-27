using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Controls;

using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
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
    
    private readonly string _shortMessage = "Are you sure you want to delete this event?";
    private string _message;
    private string? _title = "Deletion Confirmation";
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }
    
    public ICommand YesNoCommand { get; set; }

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

            _eventTypes = new ObservableCollection<AnEventType>(EventsService.GetAllEventTypes(AppSession.ServiceMode));
            _frequencies = new ObservableCollection<Frequency>(FrequencyService.GetAll(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            
            RefreshEvents();
            
            if (_events.Count > 0) 
                CurrentItem = _events.FirstOrDefault();
            else
            {
                CurrentItem = new AnEvent();
            }
            
            YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
            Icons = new ObservableCollection<MessageBoxIcon>(
                Enum.GetValues<MessageBoxIcon>());
            SelectedIcon = MessageBoxIcon.Question;
            _message = _shortMessage;
            
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    public void RefreshEvents()
    {
        Events.Clear();

        var _myEvents = EventsService.GetAllEvents(AppSession.ServiceMode);

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
                    ("Next Date", x => x.NextDate ),
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
                    ("Warning Days", x => x.WarningDays,null,null,centered),
                new TextColumn<AnEvent, long>
                    ("Days Until Next", x => x.DaysUntilNext,null,null,centered),
                new TextColumn<AnEvent, long>
                    ("Event Length", x => x.EventLength,null,null,centered),
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
        bool rtnValue;

        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {
            CurrentItem.CreationDate = DateTime.Now;
            CurrentItem.LastTriggerDate = CurrentItem.StartDate;

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
            
            if (AppSession.ServiceMode == ServiceMode.Local)
            {
                rtnValue = DataAccess.Local.AnEventRepository.Update(CurrentItem);
            }
            else
            {
                rtnValue = DataAccess.Server.AnEventRepository.Update(CurrentItem);
            }
            
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

    [RelayCommand]
    private void EditEvent()
    {
        // open the selected planting in a window for viewing/editing
        var eventWindow = new EventsWindow();
        
        if (CurrentItem != null)
        {
            var anEvent = CurrentItem;
            
            //get underlying view's viewmodel
            var vm = new EventsWindowViewModel(anEvent, this);
            
            eventWindow.DataContext = vm;
        
            eventWindow.Topmost = true;
            eventWindow.Width = 700;
            eventWindow.Height = 450;
            eventWindow.Opacity = 0.95;
            eventWindow.Title = "Event - " + anEvent.Description;
        }

        eventWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        eventWindow.Show();
    }
    
    [RelayCommand]
    private async void RemoveEvent()
    {
        if (CurrentItem != null)
        {

            await OnYesNoAsync();

            if (Result == MessageBoxResult.Yes)
            {
                //remove the record
                EventsService.DeleteEvent(AppSession.ServiceMode, CurrentItem);
                RefreshEvents();
            }
            
        }
    }
    
    private async Task Show(MessageBoxButton button)
    {
        Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
    }
}
