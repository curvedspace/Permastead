using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Services;

namespace Permastead.Views.Views;

public partial class FinderView : UserControl
{
    public FinderView()
    {
        InitializeComponent();
    }
    
    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (FinderViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (FinderViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void SearchResultsGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        var tree = this.FindControl<TreeDataGrid>("SearchResultsGrid");
        
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var aRecord = (SearchResult)current.RowSelection!.SelectedItem;

            if (aRecord != null)
            {
                switch (aRecord.Entity.Name)
                {
                    case "Action":
                        var actionWindow = new ActionWindow();
                        
                        var todo = ToDoService.GetToDoById(AppSession.ServiceMode, aRecord.Entity.Id);
                        var tdvm = new ToDoViewModel();
                        var avm = new ActionWindowViewModel(todo, tdvm);
                        
                        actionWindow.DataContext = avm;
                        
                        actionWindow.Topmost = true;
                        actionWindow.Width = 700;
                        actionWindow.Height = 450;
                        actionWindow.Opacity = 0.95;
                        actionWindow.Title = "Action Item: " + todo.Description;

                        actionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
                        actionWindow.Show();
                        break;
                    
                    case "Events":
                       
                        var eventWindow = new EventsWindow();
                        var eventId = aRecord.Entity.Id;
                        var anEvent = EventsService.GetAllEvents(AppSession.ServiceMode).Find(x => x.Id == eventId);
        
                        //get underlying view's viewmodel
                        var eventsViewModel = new EventsViewModel();
                        eventsViewModel.CurrentItem = anEvent;
                        
                        var evm = new EventsWindowViewModel(anEvent, eventsViewModel);
        
                        eventWindow.DataContext = evm;
                        eventWindow.Topmost = true;
                        eventWindow.Width = 700;
                        eventWindow.Height = 450;
                        eventWindow.Opacity = 0.95;
                        eventWindow.Title = "Event - " + anEvent.Description;
                        
                        eventWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        eventWindow.Show();
                        break;
                    
                    case "Inventory":
                        var inventoryWindow = new InventoryWindow();
                        var inventory = InventoryService.GetInventoryFromId(AppSession.ServiceMode, aRecord.Entity.Id);
                        
                        var ivm = new InventoryViewModel();
                        var iwvm = new InventoryWindowViewModel(inventory, ivm);
                        
                        inventoryWindow.DataContext = iwvm;
    
                        inventoryWindow.Topmost = true;
                        inventoryWindow.Width = 900;
                        inventoryWindow.Height = 550;
                        inventoryWindow.Opacity = 0.95;
                        inventoryWindow.Title = "Inventory Item - " + inventory.Description;
                        
                        inventoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
                        inventoryWindow.Show();
                        break;
                    
                    case "Plant" :
                       var plantWindow = new PlantWindow();
                       var plant = PlantService.GetPlantFromId(AppSession.ServiceMode, aRecord.Entity.Id);
                       var plantWindowViewModel = new PlantWindowViewModel();
                       plantWindowViewModel.Plant = plant;
                       plantWindowViewModel.RefreshData();
                      
                       plantWindow.DataContext = plantWindowViewModel;
        
                       plantWindow.Topmost = true;
                       plantWindow.Width = 900;
                       plantWindow.Height = 700;
                       plantWindow.Opacity = 0.95;
                       plantWindow.Title = "Edit Plant: " + plant.Description;
                       plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                       
                       plantWindow.Show();
                       break;
                    
                    case "Procedures":
                        var proceduresWindow = new ProceduresWindow();
                        var procedure = ProceduresService.GetFromId(AppSession.ServiceMode, aRecord.Entity.Id);
                        
                        //var pvm = new ProceduresViewModel();
                        var pwvm = new ProceduresWindowViewModel();
                        pwvm.CurrentItem = procedure;
                        
                        proceduresWindow.DataContext = pwvm;
                        
                        proceduresWindow.Topmost = true;
                        proceduresWindow.Width = 900;
                        proceduresWindow.Height = 550;
                        proceduresWindow.Opacity = 0.95;
                        proceduresWindow.Title = "Procedure Item - " + procedure.Name;
                        
                        proceduresWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        
                        proceduresWindow.Show();
                        break;
                    
                    case "Observation": 
                        var subType = aRecord.SubType;
                        var obs = ObservationsService.GetObservationById(AppSession.ServiceMode, aRecord.Entity.Id, subType);
                        
                        var vm = new ObservationWindowViewModel(obs, new ObservationsViewModel());
                        var obsWindow = new ObservationWindow();
                        obsWindow.DataContext = vm;

                        obsWindow.Topmost = true;
                        obsWindow.Width = 550;
                        obsWindow.Height = 350;
                        obsWindow.Opacity = 0.9;
                        obsWindow.Title = "Observation";
                        obsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        obsWindow.Show();
                        break;
                       
                }
            }
        }
    }
}