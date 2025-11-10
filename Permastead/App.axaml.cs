using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using LiveChartsCore; 
using LiveChartsCore.Kernel; 
using LiveChartsCore.SkiaSharpView; 

using Permastead.Themes;
using Permastead.ViewModels;
using Permastead.Views;

using Serilog;
using Services;

namespace Permastead;

public partial class App : Application
{
    public static IThemeManager? ThemeManager;
    public override void Initialize()
    {
        // ThemeManager = new FluentThemeManager();
        // ThemeManager.Initialize(this);

        AvaloniaXamlLoader.Load(this);

        if (AppSession.ServiceMode == ServiceMode.Local)
        {
            if (!DataAccess.Local.HomesteaderRepository.DatabaseExists())
            {
                Log.Logger.Information("New SQLite database created.");
            }
        }
        else
        {
            if (!DataAccess.Server.HomesteaderRepository.DatabaseExists())
            {
                Log.Logger.Information("New Postgresql database created.");
            }
        }
        
        
        LiveCharts.Configure(config => 
                config 
                    // you can override the theme 
                    .AddDefaultTheme()  
            
        ); 
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
