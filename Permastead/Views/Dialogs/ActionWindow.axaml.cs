using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Dialogs;

public partial class ActionWindow : Window
{
    public ActionWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var vm  = (ActionWindowViewModel)DataContext;
        vm.SaveData();
        
        Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}