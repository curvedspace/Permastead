using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Permastead.Views;

public partial class GaiaWindow : Window
{
    public GaiaWindow()
    {
        InitializeComponent();
        
        App.ThemeManager?.Switch(1);
    }
}