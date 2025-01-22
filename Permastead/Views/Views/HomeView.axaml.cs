using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
