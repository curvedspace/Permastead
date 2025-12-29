using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Ursa.Controls;

namespace Permastead.Views.Dialogs;

public partial class PlantWindow : Window
{
    private PlantWindowViewModel? _viewModel;
    
    public PlantWindow()
    {
        InitializeComponent();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not PlantWindowViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    { 
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        PlantWindowViewModel vm  = (PlantWindowViewModel)DataContext;
        vm.SavePlant();
        vm.RefreshData();
        
        Close();
    }
}