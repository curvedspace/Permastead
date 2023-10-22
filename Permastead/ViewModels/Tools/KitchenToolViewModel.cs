using Dock.Model.Mvvm.Controls;
using Permastead.ViewModels.Views;

namespace Permastead.ViewModels.Tools;

public class KitchenToolViewModel : Tool
{
    public HomeViewModel Home { get; set; }
    public DockFactory Dock { get; set; }
}
