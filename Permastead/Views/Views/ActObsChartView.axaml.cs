using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class ActObsChartView : UserControl
{
    public ActObsChartView()
    {
        InitializeComponent();
        DataContext = new ActObsChartViewModel();
    }
}