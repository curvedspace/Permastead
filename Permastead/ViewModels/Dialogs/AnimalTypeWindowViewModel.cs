using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class AnimalTypeWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private AnimalType _animalType;
    
    private AnimalsViewModel _controlViewModel { get; set;  } = new AnimalsViewModel();

    public AnimalTypeWindowViewModel(AnimalType animalType, AnimalsViewModel controlViewModel) : this(controlViewModel)
    {
        _animalType = animalType;
    }

    public AnimalTypeWindowViewModel(AnimalsViewModel controlViewModel)
    {
        AnimalType = new AnimalType();
        _controlViewModel = controlViewModel;
    }
    
    
    public void SaveRecord()
    {
        //if there is a comment, save it.

        if (_animalType != null && _animalType.Id == 0 && !string.IsNullOrEmpty(_animalType.Description))
        {

            _animalType.CreationDate = DateTime.Now;
            
            var rtnValue = AnimalService.CommitAnimalTypeRecord(AppSession.ServiceMode, _animalType);
            Log.Logger.Information("Animal Type: " + _animalType.Description + " saved: " + rtnValue);

        }
        else
        {
            var rtnValue = AnimalService.CommitAnimalTypeRecord(AppSession.ServiceMode, _animalType);
            
        }
       
        //need a way to send a refresh message back to the tree browser...
        if (_controlViewModel != null) _controlViewModel.RefreshDataOnly();

    }

}