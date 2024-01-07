using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class ContactsView : UserControl
{
    public ContactsView()
    {
        InitializeComponent();
        DataContext = new ContactsViewModel();
    }

    private void TreeDataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        // open the selected planting in a window for viewing/editing
        var personWindow = new PersonWindow();
        
        //get the selected row in the list
        var current = sender as TreeDataGrid;
        if (current != null)
        {
            var person = (Person)current.RowSelection!.SelectedItem;
            
            //get underlying view's viewmodel
            var vm = new PersonWindowViewModel(person, (ContactsViewModel)DataContext);
            
            personWindow.DataContext = vm;
        
            personWindow.Topmost = true;
            personWindow.Width = 900;
            personWindow.Height = 550;
            personWindow.Opacity = 0.95;
            personWindow.Title = "Person - " + person.FullNameLastFirst;
        }

        personWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
        personWindow.Show();
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        var win = new PersonWindow();
        var vm = new PersonWindowViewModel(new Person(), (ContactsViewModel)DataContext);
            
        win.DataContext = vm;
        
        win.Topmost = true;
        win.Width = 900;
        win.Height = 550;
        win.Opacity = 0.95;
        win.Title = "New Person";
        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
        win.Show();
    }
}