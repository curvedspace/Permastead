using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Permastead.Views.Views;

public partial class SettingsPageView : UserControl
{
    public SettingsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}