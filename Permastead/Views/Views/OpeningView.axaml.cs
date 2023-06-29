using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class OpeningView : UserControl
{
    private OpeningViewModel _openingViewModel;
    
    public OpeningView()
    {
        InitializeComponent();
        
        _openingViewModel = new OpeningViewModel();
        _openingViewModel.GetQuote();

        DataContext = _openingViewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}