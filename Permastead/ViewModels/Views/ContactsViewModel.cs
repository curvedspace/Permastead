using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Services;
using Ursa.Controls;

namespace Permastead.ViewModels.Views;

public partial class ContactsViewModel : ViewModelBase
{
    private ObservableCollection<Person> _people = new ObservableCollection<Person>();
    
    [ObservableProperty]
    private ObservableCollection<PersonObservation> _peopleObservations = new ObservableCollection<PersonObservation>();
    
    [ObservableProperty] 
    private Person _currentPerson;
    
    [ObservableProperty] 
    private long _peopleCount;

    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty] private PersonObservation _currentObservation = new PersonObservation();

    public FlatTreeDataGridSource<Person> PersonSource { get; set; }
    
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this contact record?";
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
    
    public WindowToastManager? ToastManager { get; set; }
    
    public ContactsViewModel()
    {
        RefreshDataOnly();
        GetPeopleObservations();
        
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.Question;
        _message = _shortMessage;
    }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }

    public void GetPeopleObservations()
    {
        if (CurrentPerson != null)
        {
            PeopleObservations =
                new ObservableCollection<PersonObservation>(
                    Services.PersonService.GetObservationsForPerson(AppSession.ServiceMode, CurrentPerson.Id));
        }
    }
    
    public void RefreshDataOnly()
    {
        var myPeople = Services.PersonService.GetAllPeople(AppSession.ServiceMode);

        _people.Clear();
        foreach (var p in myPeople)
        {
            _people.Add(p);
            if (CurrentPerson == null) CurrentPerson = p;
        }
        
        PersonSource = new FlatTreeDataGridSource<Person>(_people)
        {
            Columns =
            {
                new TextColumn<Person, string>
                    ("Last Name", x => x.LastName),
                new TextColumn<Person, string>
                    ("First Name", x => x.FirstName),
                new TextColumn<Person, string>
                    ("Company", x => x.Company),
                new TextColumn<Person, string>
                    ("Email", x => x.Email),
                new TextColumn<Person, string>
                    ("Phone", x => x.Phone),
                new TextColumn<Person, string>
                    ("Address", x => x.Address),
                new TextColumn<Person, string>
                    ("Tags", x => x.Tags),
                new TextColumn<Person, string>
                    ("Comment", x => x.Comment)
            },
        };

        PeopleCount = _people.Count;
    }
    
    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveEvent()
    {
        //if there is a comment, save it.
        
    }
    
    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author!.Id = AppSession.Instance.CurrentUser.Id;
            CurrentObservation.Person = CurrentPerson;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 2;
            
            PersonService.AddPersonObservation(AppSession.ServiceMode, CurrentObservation);
            
            PeopleObservations =
                new ObservableCollection<PersonObservation>(
                    Services.PersonService.GetObservationsForPerson(AppSession.ServiceMode, CurrentPerson.Id));

            CurrentObservation = new PersonObservation();
            
            ToastManager?.Show(new Toast("Observation saved!"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    [RelayCommand]
    private void AddContact()
    {
        
    }

    [RelayCommand]
    private void EditContact()
    {
        // open the selected planting in a window for viewing/editing
        var personWindow = new PersonWindow();
        
        //get the selected row in the list
        if (CurrentPerson != null)
        {
            
            //get underlying view's viewmodel
            var vm = new PersonWindowViewModel(CurrentPerson, this);
            
            personWindow.DataContext = vm;
        
            personWindow.Topmost = true;
            personWindow.Width = 900;
            personWindow.Height = 550;
            personWindow.Opacity = 0.95;
            personWindow.Title = "Person - " + CurrentPerson.FullNameLastFirst;
        }

        personWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        personWindow.Show();
    }
    
    [RelayCommand]
    private async void DeleteContact()
    {
        try
        {
            if (CurrentPerson != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    //remove the record
                    Services.PersonService.DeleteRecord(AppSession.ServiceMode, CurrentPerson);
                    RefreshDataOnly();
                }
            }
        }
        catch (Exception e)
        {
        }
    }
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task Show(MessageBoxButton button)
    {
        Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
    }
    
}