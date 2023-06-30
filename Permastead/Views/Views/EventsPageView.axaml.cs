using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Permastead.Views.Views;

public partial class EventsPageView : UserControl
{
    public EventsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}