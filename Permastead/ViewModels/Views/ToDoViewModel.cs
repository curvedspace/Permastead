
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class ToDoViewModel : ViewModelBase
{
    
    [ObservableProperty] 
    private ObservableCollection<ToDo> _todos;

    [ObservableProperty] 
    private ObservableCollection<ToDoType> _todoTypes;
    
    [ObservableProperty]
    private ObservableCollection<ToDoStatus> _todoStatuses;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;

    [ObservableProperty] 
    private long _activeToDos;

    [ObservableProperty] 
    private long _toDoCount;
    
    [ObservableProperty] 
    private bool _activeOnly = true;
    
    [ObservableProperty] 
    private ToDo _currentItem;


    [RelayCommand]
    // The method that will be executed when the command is invoked
    private void SaveToDo()
    {
        //if there is a comment, save it.

        if (CurrentItem != null && CurrentItem.Id == 0 && !string.IsNullOrEmpty(CurrentItem.Description))
        {

            CurrentItem.CreationDate = DateTime.Now;
 
            var rtnValue = DataAccess.Local.ToDoRepository.Insert(CurrentItem);

            if (rtnValue)
            {
                _todos.Add(CurrentItem);
            }

            Console.WriteLine("saved " + rtnValue);

            RefreshToDo();
        }
        else
        {
            var rtnValue = DataAccess.Local.ToDoRepository.Update(_currentItem);
            RefreshToDo();
        }

    }

    [RelayCommand]
    private void ResetToDo()
    {
        RefreshToDo();
        
        // reset the current item
        CurrentItem = new ToDo();
        OnPropertyChanged(nameof(CurrentItem));
        
    }

    [RelayCommand]
    private void RefreshToDo()
    {
        _todos.Clear();
        _activeToDos = 0;

        List<ToDo> todos = null;

        if (ActiveOnly)
        {
            todos = Services.ToDoService.GetActiveTodos(AppSession.ServiceMode);
        }
        else
        {
            todos = Services.ToDoService.GetAllToDos(AppSession.ServiceMode);
        }
        

        foreach (var todo in todos)
        {
            todo.ToDoStatus = TodoStatuses.First(x => x.Id == todo.ToDoStatus.Id);
            todo.ToDoType = TodoTypes.First(x => x.Id == todo.ToDoType.Id);
            todo.Assignee = People.First(x => x.Id == todo.Assignee.Id);
            todo.Assigner = People.First(x => x.Id == todo.Assigner.Id);
            
            Todos.Add(todo);
            if (todo.ToDoStatus.Description != "Complete") ActiveToDos = ActiveToDos + 1;
            ToDoCount = Todos.Count;
        }
    }
    

    public ToDoViewModel()
    {
        try
        {
            _todoTypes = new ObservableCollection<ToDoType>();
            _todoStatuses = new ObservableCollection<ToDoStatus>();
            _people = new ObservableCollection<Person>();
            _todos = new ObservableCollection<ToDo>();
            
            _currentItem = new ToDo();

            _todoTypes = new ObservableCollection<ToDoType>(Services.ToDoService.GetAllToDoTypes(AppSession.ServiceMode));
            _todoStatuses = new ObservableCollection<ToDoStatus>(Services.ToDoService.GetAllToDoStatuses(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            
            RefreshToDo();
            
            if (_todos.Count > 0) CurrentItem = _todos.FirstOrDefault();

            
        }
        catch (Exception)
        {
            throw;
        }
    }
}
    