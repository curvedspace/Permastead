
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;
using Ursa.Controls;

namespace Permastead.Views.Views;

public partial class ToDoView : UserControl
{
    private ToDoViewModel? _viewModel;
    public ToDoView()
    {
        InitializeComponent();
        DataContext = new ToDoViewModel();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not ToDoViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
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

    private void TodoGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var actionWindow = new ActionWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var todo = (ToDo)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new ActionWindowViewModel(todo, (ToDoViewModel)DataContext);
            //var vm = new ActionWindowViewModel();
            
            actionWindow.DataContext = vm;
        
            actionWindow.Topmost = true;
            actionWindow.Width = 700;
            actionWindow.Height = 450;
            actionWindow.Opacity = 0.95;
            actionWindow.Title = "Action Item - Due in " + todo.DaysUntilDue + " day(s).";
        }

        actionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        actionWindow.Show();
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new ActionWindow();
        var vm = new ActionWindowViewModel();
        vm.ControlViewModel = (ToDoViewModel)DataContext;

            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 700;
        win.Height = 450;
        win.Opacity = 0.95;
        win.Title = "New Action Item";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
        
    }
}