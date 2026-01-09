using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;
using Ursa.Controls;

namespace Permastead.Views.Views;

public partial class HomeView : UserControl
{
    private HomeViewModel? _viewModel;
    
    public HomeView()
    {
        InitializeComponent();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not HomeViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
