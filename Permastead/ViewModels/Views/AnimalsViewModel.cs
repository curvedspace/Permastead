using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class AnimalsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Animal> _animals = new ObservableCollection<Animal>();

    [ObservableProperty]
    private ObservableCollection<AnimalType> _animalTypes = new ObservableCollection<AnimalType>();
    
    [ObservableProperty] 
    private bool _currentOnly = true;

    [ObservableProperty]
    private long _animalCount;

    [ObservableProperty]
    private Animal _currentItem;
    
    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty] private AnimalObservation _currentObservation = new AnimalObservation();
    
    [ObservableProperty]
    private ObservableCollection<AnimalObservation> _animalObservations = new ObservableCollection<AnimalObservation>();
    
    [ObservableProperty]
    private string _searchText = "";
    
    public FlatTreeDataGridSource<Animal> AnimalSource { get; set; }
    
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }
    
    public void GetAnimalObservations()
    {
        if (CurrentItem != null)
        {
            AnimalObservations =
                new ObservableCollection<AnimalObservation>(
                    Services.AnimalService.GetObservationsForAnimal(AppSession.ServiceMode, CurrentItem.Id));
        }
    }
    
    [RelayCommand]
    public void RefreshDataOnly(string filterText = "")
    {
        Animals.Clear();
        AnimalTypes.Clear();
        
        var animals = Services.AnimalService.GetAllAnimals(AppSession.ServiceMode);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();
        
        foreach (var animal in animals)
        {
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                if (CurrentOnly)
                {
                    if (animal.IsCurrent()) Animals.Add(animal);
                }
                else
                {
                    Animals.Add(animal);
                }
            }
            else
            {
                if (animal.Name.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    animal.NickName.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    animal.Type.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    animal.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    if (CurrentOnly)
                    {
                        if (animal.IsCurrent()) Animals.Add(animal);
                    }
                    else
                    {
                        Animals.Add(animal);
                    }
                }
            }
        }
        
        AnimalSource = new FlatTreeDataGridSource<Animal>(Animals)
        {
            Columns =
            {
                new TextColumn<Animal, string>
                    ("Date", x => x.StartDateString),
                new TextColumn<Animal, string>
                    ("Name", x => x.Name),
                new TextColumn<Animal, string>
                    ("NickName", x => x.NickName),
                new TextColumn<Animal, string>
                    ("Author", x => x.Author.FirstName),
                new TextColumn<Animal, string>
                    ("Type", x => x.Type.Description),
                new TextColumn<Animal, string>
                    ("Breed", x => x.Breed),
                new TextColumn<Animal, string>
                    ("Birthday", x => x.BirthdayString),
                new TextColumn<Animal, string>
                    ("Comment", x => x.Comment)
            },
        };
        
        if (Animals.Count > 0) 
            CurrentItem = Animals.FirstOrDefault()!;
        else
            CurrentItem = new Animal();

        AnimalCount = Animals.Count;
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author = AppSession.Instance.CurrentUser;
            CurrentObservation.Animal = CurrentItem;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 2;
            
            AnimalService.AddAnimalObservation(AppSession.ServiceMode, CurrentObservation);
            
            AnimalObservations =
                new ObservableCollection<AnimalObservation>(Services.AnimalService.GetObservationsForAnimal(AppSession.ServiceMode, CurrentItem.Id));

            CurrentObservation = new AnimalObservation();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    public AnimalsViewModel()
    {
        RefreshDataOnly();
    }
}