using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Local;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class ActionWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ToDo _currentTodo;
    
    [ObservableProperty] 
    private ObservableCollection<ToDoType> _todoTypes;
    
    [ObservableProperty]
    private ObservableCollection<ToDoStatus> _todoStatuses;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    public ToDoViewModel ControlViewModel { get; set;  } = new ToDoViewModel();

    public ActionWindowViewModel()
    {
        _todoTypes = new ObservableCollection<ToDoType>();
        _todoStatuses = new ObservableCollection<ToDoStatus>();
        _people = new ObservableCollection<Person>();
            
        _currentTodo = new ToDo();

        _todoTypes = new ObservableCollection<ToDoType>(ToDoService.GetAllToDoTypes(AppSession.ServiceMode));
        _todoStatuses = new ObservableCollection<ToDoStatus>(ToDoService.GetAllToDoStatuses(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
    }
    
    public ActionWindowViewModel(ToDo todo, ToDoViewModel obsVm) : this()
    {
        _currentTodo = todo;
        ControlViewModel = obsVm;

        if (_currentTodo != null)
        {
            _currentTodo.ToDoStatus = TodoStatuses.First(x => x.Id == todo.ToDoStatus.Id);
            _currentTodo.ToDoType = TodoTypes.First(x => x.Id == todo.ToDoType.Id);
            _currentTodo.Assignee = People.First(x => x.Id == todo.Assignee.Id);
            _currentTodo.Assigner = People.First(x => x.Id == todo.Assigner.Id);
        }
        
    }

    public void SaveData()
    {
        bool rtnValue;
        
        rtnValue = Services.ToDoService.CommitRecord(AppSession.ServiceMode, _currentTodo);
        
        OnPropertyChanged(nameof(_currentTodo));
            
        using (LogContext.PushProperty("ActionWindowViewModel", this))
        {
            Log.Information("Saved Action: " + _currentTodo.Description, rtnValue);
        }
        
        ControlViewModel.SaveData();
    }
}