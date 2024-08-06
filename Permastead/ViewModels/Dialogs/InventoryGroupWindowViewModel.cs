using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace Permastead.ViewModels.Dialogs;

public partial class InventoryGroupWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private InventoryGroup _group;
    
    [ObservableProperty] 
    private ObservableCollection<Person> _people;
}
