
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class ObservationWindow : Window
{
    public ObservationWindow()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ObservationWindowViewModel vm  = (ObservationWindowViewModel)DataContext;
        vm.SaveObservation();
        
        Close();
    }
}