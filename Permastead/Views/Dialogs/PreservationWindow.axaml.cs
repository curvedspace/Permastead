using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class PreservationWindow : Window
{
    public PreservationWindow()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PreservationWindowViewModel vm  = (PreservationWindowViewModel)DataContext;
        vm.SaveRecord();
        
        Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}