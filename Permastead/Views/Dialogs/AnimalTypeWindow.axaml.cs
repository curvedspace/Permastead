using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;

namespace Permastead.Views.Dialogs;

public partial class AnimalTypeWindow : Window
{
    public AnimalTypeWindow()
    {
        InitializeComponent();
    }
    
    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AnimalTypeWindowViewModel vm  = (AnimalTypeWindowViewModel)DataContext;
        vm.SaveRecord();
        
        Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}