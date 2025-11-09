using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ConnectionNodes : UserControl
{
    public ConnectionNodes()
    {
        InitializeComponent();
        DataContext = new ConnectionNodesViewModel();
    }
}