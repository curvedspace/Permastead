using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class PlantWindow : Window
{
    public PlantWindow()
    {
        InitializeComponent();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantWindowViewModel vm  = (PlantWindowViewModel)DataContext;
        vm.SavePlant();
        
        Close();
    }
}