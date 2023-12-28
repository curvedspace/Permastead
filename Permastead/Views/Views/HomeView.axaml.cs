using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class HomeView : UserControl
{
    private HomeViewModel _homeViewModel;
    
    public HomeView()
    {
        InitializeComponent();
        
        _homeViewModel = new HomeViewModel();
        _homeViewModel.GetQuote();
        
        DataContext = _homeViewModel;
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
