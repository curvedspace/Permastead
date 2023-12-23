using System;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace Permastead.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        //InitializeThemes();
        //InitializeMenu();

        App.ThemeManager?.Switch(1);
    }

    private void InitializeThemes()
    {
        var dark = true;
        var theme = this.Find<Button>("ThemeButton");
        if (theme is { })
        {
            theme.Click += (_, _) =>
            {
                dark = !dark;
                App.ThemeManager?.Switch(dark ? 1 : 0);
            };
        }
    }
    
}