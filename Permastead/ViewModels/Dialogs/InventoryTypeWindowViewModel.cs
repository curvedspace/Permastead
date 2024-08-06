using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryTypeWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private InventoryType _type;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
}