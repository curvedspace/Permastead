
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ToDoView : UserControl
{
    public ToDoView()
    {
        InitializeComponent();
        DataContext = new ToDoViewModel();
    }
    
}