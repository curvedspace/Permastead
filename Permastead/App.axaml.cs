﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Permastead.Themes;
using Permastead.ViewModels;
using Permastead.Views;
using Serilog;

namespace Permastead;

public partial class App : Application
{
    public static IThemeManager? ThemeManager;
    public override void Initialize()
    {
        // ThemeManager = new FluentThemeManager();
        // ThemeManager.Initialize(this);

        AvaloniaXamlLoader.Load(this);
        
        if (!DataAccess.Local.HomesteaderRepository.DatabaseExists())
        {
            Log.Logger.Information("New database created.");
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        //BindingPlugins.DataValidators.RemoveAt(0);

        var mainWindowViewModel = new MainWindowViewModel();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktopLifetime:
            {
                var mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };

                // mainWindow.Closing += (_, _) =>
                // {
                //     mainWindowViewModel.CloseLayout();
                // };
                //
                desktopLifetime.MainWindow = mainWindow;
                //
                // desktopLifetime.Exit += (_, _) =>
                // {
                //     mainWindowViewModel.CloseLayout();
                // };

                break;
            }
            case ISingleViewApplicationLifetime singleViewLifetime:
            {
                var mainView = new MainView()
                {
                    DataContext = mainWindowViewModel
                };

                singleViewLifetime.MainView = mainView;

                break;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
