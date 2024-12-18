using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Models;
using Permastead.ViewModels.Dialogs;
using Services;

namespace Permastead.Views.Dialogs;

public partial class HarvestWindow : Window
{
    public HarvestWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        //load aux values based on harvest type
        if (sender as ComboBox != null && ((ComboBox)sender).IsLoaded == true)
        {
            
            var ctrl = sender as ComboBox;
            var harvestType = ctrl.SelectedItem as HarvestType;
            
            if (harvestType != null)
                Console.WriteLine("Selection changed: " + harvestType.Description);
            
            // load the aux values (entities) based on the harvest type
            var vm  = (HarvestWindowViewModel)DataContext;
            vm.Entities.Clear();

            switch (harvestType.Description)
            {
                case "Animal":
                    vm.Entities = new ObservableCollection<Entity>(vm.AnimalsList);
                    break;
                
                case "Plant":
                    vm.Entities = new ObservableCollection<Entity>(vm.PlantingsList);
                    break;
                
                default:
                    vm.Entities = new ObservableCollection<Entity>(vm.OtherList);
                    break;
            }
        }
            
    }
}