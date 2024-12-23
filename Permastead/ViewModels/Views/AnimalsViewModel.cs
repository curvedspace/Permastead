using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
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
                    ("Author", x => x.Author!.FirstName),
                new TextColumn<Animal, string>
                    ("Type", x => x.Type.Description),
                new TextColumn<Animal, string>
                    ("Breed", x => x.Breed),
                new TextColumn<Animal, string>
                    ("Birthday", x => x.BirthdayString),
                new CheckBoxColumn<Animal>
                (
                    "Is Pet",
                    x => x.IsPet,
                    (o, v) => o.IsPet = v,
                    options: new()
                    {
                        CanUserResizeColumn = false, CanUserSortColumn = true
                    }),
                
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

    [RelayCommand]
    public void EditAnimal()
    {
        // open the selected record in a window for viewing/editing
        var myWindow = new AnimalWindow();
        
        
        if (CurrentItem != null)
        {
            //get underlying view's viewmodel
            var vm = new AnimalWindowViewModel(CurrentItem, this);
            
            myWindow.DataContext = vm;
        
            myWindow.Topmost = true;
            myWindow.Width = 800;
            myWindow.Height = 550;
            myWindow.Opacity = 0.95;
            myWindow.Title = "Animal - " + CurrentItem.Name;
        }

        myWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        myWindow.Show();
        
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    public void AddRecord()
    {
        var myAnimal = new Animal();
        myAnimal.Author = AppSession.Instance.CurrentUser;
        myAnimal.StartDate = DateTime.Today;
        myAnimal.Birthday = DateTime.Today;
        myAnimal.EndDate = DateTime.MaxValue;
        
        
        var vm = new AnimalWindowViewModel(myAnimal, this);
            
        var win = new AnimalWindow();
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 800;
        win.Height = 550;
        win.Opacity = 0.95;
        win.Title = "New Animal";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
    
    [RelayCommand]
    private void HarvestAnimal()
    {
        try
        {
            // open the selected planting in a harvest window 
            var harvestWindow = new HarvestWindow();
        
            //get the selected row in the list
            var current = CurrentItem;
        
            if (current != null)
            {
                var harvest = new Harvest();
                harvest.Author = AppSession.Instance.CurrentUser;
                harvest.Description = current.Name;
            
                var harvestTypes = HarvestService.GetAllHarvestTypes(AppSession.ServiceMode);
                var plantType = harvestTypes.FirstOrDefault(x => x.Description!.ToLowerInvariant() == "animal");

                harvest.HarvestType = plantType;
                harvest.HarvestEntity.Id = current.Id;

                var hvm = new HarvestsViewModel();
                hvm.CurrentItem = harvest;
            
            
                var vm = new HarvestWindowViewModel(harvest, hvm);

                harvestWindow.DataContext = vm;

                harvestWindow.Topmost = true;
                harvestWindow.Width = 1000;
                harvestWindow.Height = 600;
                harvestWindow.Opacity = 0.95;
                harvestWindow.Title = "Harvest - " + current.Name;
                harvestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                harvestWindow.Show();
            }
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