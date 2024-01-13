
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ToDoView : UserControl
{
    public ToDoView()
    {
        InitializeComponent();
        DataContext = new ToDoViewModel();
    }

    private void TodoGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var vm = (ToDoViewModel)DataContext;
            vm.CurrentItem = (ToDo)current.RowSelection!.SelectedItem;
        }
    }
}