using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Permastead.Views.Views;

public partial class PlantingsPageView : UserControl
{
    public PlantingsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}