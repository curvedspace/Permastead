using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void TreeDataGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var vm = (InventoryViewModel)DataContext;
            
            vm.CurrentItem = (Inventory)current.RowSelection!.SelectedItem;
        }
    }
}