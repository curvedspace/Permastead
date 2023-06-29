using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void Button_OnClickSave(object? sender, RoutedEventArgs e)
    {
        
        // var mainWindow = this.Parent.Find<MainWindow>("MainWindow");
        //
        // if(mainWindow != null)
        //     MessageBox.Success(mainWindow, "Saved","Your settings have been saved", WindowStartupLocation.CenterOwner);
    }
}