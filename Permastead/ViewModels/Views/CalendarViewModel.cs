
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class CalendarViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<Observation> _observations = new ObservableCollection<Observation>();
    
    public CalendarViewModel()
    {
        
    }
}