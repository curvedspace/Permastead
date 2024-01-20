using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class EventsWindow : Window
{
    public EventsWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        EventsWindowViewModel vm  = (EventsWindowViewModel)DataContext;
        vm.SaveRecord();
        
        Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}