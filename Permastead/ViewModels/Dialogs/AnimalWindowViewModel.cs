using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class AnimalWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Animal _currentItem;
    
    [ObservableProperty]
    private ObservableCollection<AnimalType> _animalTypes;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
    
    public AnimalsViewModel ControlViewModel { get; set;  } = new AnimalsViewModel();
    
    public AnimalWindowViewModel()
    {
        _animalTypes = new ObservableCollection<AnimalType>();
        _people = new ObservableCollection<Person>();
            
        _currentItem = new Animal();

        _animalTypes = new ObservableCollection<AnimalType>(AnimalService.GetAllAnimalTypes(AppSession.ServiceMode));
        _people = new ObservableCollection<Person>(PersonService.GetAllPeople(AppSession.ServiceMode));
    }
    
    public AnimalWindowViewModel(Animal item, AnimalsViewModel obsVm) : this()
    {
        CurrentItem = item;
        ControlViewModel = obsVm;
        
        CurrentItem = item;
        
        if (CurrentItem != null)
        {
            CurrentItem.Type = AnimalTypes.First(x => x.Id == CurrentItem.AnimalTypeId);
            CurrentItem.Author = People.First(x => x.Id == CurrentItem.Author.Id);
        }
        
    }
}