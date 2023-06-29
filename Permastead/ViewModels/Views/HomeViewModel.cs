using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using System.Diagnostics;

namespace Permastead.ViewModels.Views;

public partial class HomeViewModel : RootDock
{
    [RelayCommand]
    private void OpenPlant()
    {
        Debug.WriteLine("Open Plant");
    }

}
