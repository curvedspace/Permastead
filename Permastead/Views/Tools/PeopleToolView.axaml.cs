using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.Models.Tools;
using Permastead.ViewModels.Tools;

namespace Permastead.Views.Tools;

public partial class PeopleToolView : UserControl
{
    public PeopleToolView()
    {
        InitializeComponent();
    }
    
    private void PeopleListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            ((PeopleToolViewModel)(this.DataContext)).Open();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        
    }
}
