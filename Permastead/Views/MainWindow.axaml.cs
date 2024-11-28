using System;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;

namespace Permastead.Views;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        
        try
        {
                
            this.Title = $"Permastead V{Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version} - " +
                         File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("yyyy-MM-dd");

#if DEBUG
            this.AttachDevTools();
#endif
            
        }
        catch (Exception ex)
        {
            this.Title = $"Permastead (" + DateTime.Today.ToString("yyyy-MM-dd") + ")";
            Console.WriteLine(ex.Message);
        }
    }
}
