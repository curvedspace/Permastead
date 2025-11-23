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
                   case "Plant" :
                       var plantWindow = new PlantWindow();
                       var plant = PlantService.GetPlantFromId(AppSession.ServiceMode, aRecord.Entity.Id);
                       var plantWindowViewModel = new PlantWindowViewModel();
                       plantWindowViewModel.Plant = plant;
                       plantWindow.DataContext = plantWindowViewModel;
        
                       plantWindow.Topmost = true;
                       plantWindow.Width = 900;
                       plantWindow.Height = 700;
                       plantWindow.Opacity = 0.95;
                       plantWindow.Title = "Edit Plant";
                       plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                       plantWindow.Show();
                       break;
                       
                }
            }
        }
    }
}