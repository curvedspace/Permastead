using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class PlantingsView : UserControl
{
    public PlantingsView()
    {
        InitializeComponent();
        DataContext = new PlantingsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}