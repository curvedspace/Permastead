using System;
using System.Runtime.InteropServices;
using Avalonia;

using Serilog;

namespace Permastead.Desktop;

public class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            
        }
       
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
    {
        try
        {
            
            var appBuilder = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
            
            //create logging 
            string logFilename = string.Empty;
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                
            if (isWindows)
            {
                logFilename = userFolder + @"\.config\permastead\log.txt";
            }
            else
            {
                //linux or macos
                logFilename = userFolder + @"/.config/permastead/log.txt";
            }
                
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(logFilename,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
                
            Log.Information("Starting Permastead");

            return appBuilder;
            
        }
        finally
        {
            
        }
        
    }
}
