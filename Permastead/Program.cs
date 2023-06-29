using Avalonia;
using Avalonia.Logging;

using Serilog;
using System;
using System.Runtime.InteropServices;

namespace Permastead
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {

            var appBuilder = AppBuilder.Configure<App>()
                .LogToTrace(LogEventLevel.Information);

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
            
           
            // Log.CloseAndFlush();

            return appBuilder;
        }
    }
}