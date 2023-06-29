using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.Models.Tools;
using Permastead.ViewModels.Tools;

namespace Permastead.Views.Tools;

public partial class Tool1View : UserControl
{
    public Tool1View()
    {
        InitializeComponent();
    }
    
    private void PlantsListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            ((Tool1ViewModel)(this.DataContext)).Open();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        
    }
}
