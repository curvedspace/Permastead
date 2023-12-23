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
        
        this.FindControl<ListBox>("GaiaListBox").PropertyChanged += ListBoxPropertyChanged;
        //RequestTextBox.PropertyChanged += TextBoxPropertyChanged;
    }


    private void InitializeComponent()
    {
        try
        {
            AvaloniaXamlLoader.Load(this);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    private void ListBoxPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        //Console.WriteLine(e.Property.Name);
        if(e.Property.Name == "ItemCount")
        {
            var itemCount = this.FindControl<ListBox>("GaiaListBox").ItemCount;
            this.FindControl<ListBox>("GaiaListBox").SelectedIndex = itemCount-1;
            this.FindControl<ListBox>("GaiaListBox").ScrollIntoView(itemCount);
            // this.FindControl<ListBox>("GaiaListBox").InvalidateVisual();
            
        }
    }
}