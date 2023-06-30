using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class EventsView : UserControl
{
    public EventsView()
    {
        InitializeComponent();
        DataContext = new EventsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}