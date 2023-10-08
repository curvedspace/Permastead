using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ContactsView : UserControl
{
    public ContactsView()
    {
        InitializeComponent();
        DataContext = new ContactsViewModel();
    }
}