using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class DashboardView : UserControl
{
    private DashboardViewModel _dashboardViewModel;
    
    public DashboardView()
    {
        InitializeComponent();
        
        _dashboardViewModel = new DashboardViewModel();
        _dashboardViewModel.GetQuote();
        
        DataContext = _dashboardViewModel;
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
