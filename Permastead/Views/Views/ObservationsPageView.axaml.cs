using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Permastead.Views.Views;

public partial class ObservationsPageView : UserControl
{
    public ObservationsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}