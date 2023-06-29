using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ObservationsView : UserControl
{
    public ObservationsView()
    {
        InitializeComponent();
        DataContext = new ObservationsViewModel();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}
