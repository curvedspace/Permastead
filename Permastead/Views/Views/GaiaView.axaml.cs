using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;
using System;

namespace Permastead.Views.Views;

public partial class GaiaView : UserControl
{
    public GaiaView()
    {
        InitializeComponent();
        DataContext = new GaiaViewModel();
    }


    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}