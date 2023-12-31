
using System;
using System.Collections.Generic;
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
    
    public FlatTreeDataGridSource<ToDo> ToDosSource { get; set; }


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
            todos = ToDoService.GetActiveTodos(AppSession.ServiceMode);
        }
        else
        {
            todos = ToDoService.GetAllToDos(AppSession.ServiceMode);
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
        
        var centered = new TextColumnOptions<ToDo>
        {
            TextTrimming = TextTrimming.None,
            TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
        };
        
        ToDosSource = new FlatTreeDataGridSource<ToDo>(_todos)
        {
            Columns =
            {
                new TextColumn<ToDo, string>
                    ("Due Date", x => x.DisplayDueDate),
                new TextColumn<ToDo, long>
                    ("Days Until Due", x => x.DaysUntilDue, GridLength.Auto,centered),
                new TextColumn<ToDo, string>
                    ("Type", x => x.ToDoType.Description),
                new TextColumn<ToDo, string>
                    ("Description", x => x.Description),
                new TextColumn<ToDo, string>
                    ("Assigner", x => x.Assigner.FirstName),
                new TextColumn<ToDo, string>
                    ("Assignee", x => x.Assignee.FirstName),
                new TextColumn<ToDo, string>
                    ("Status", x => x.ToDoStatus.Description),
                new TextColumn<ToDo, int>
                    ("% Done", x => x.PercentDone, GridLength.Auto, centered),
                new TextColumn<ToDo, DateTime>
                    ("Last Updated", x => x.LastUpdatedDate)
            },
        };
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

            _todoTypes = new ObservableCollection<ToDoType>(ToDoService.GetAllToDoTypes(AppSession.ServiceMode));
            _todoStatuses = new ObservableCollection<ToDoStatus>(ToDoService.GetAllToDoStatuses(AppSession.ServiceMode));
            _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
            
            RefreshToDo();
            
            if (Todos.Count > 0) CurrentItem = Todos.FirstOrDefault();

            
        }
        catch (Exception)
        {
            throw;
        }
    }
}
    