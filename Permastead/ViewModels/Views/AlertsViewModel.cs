using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace Permastead.ViewModels.Views;

public partial class AlertsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<AlertItem> _alerts = new ObservableCollection<AlertItem>();
    
    
    [RelayCommand]
    private void RefreshData()
    {
        Alerts.Clear();
        Alerts = new ObservableCollection<AlertItem>(AppSession.Instance.AlertManager.Alerts.Values);
    }

    public AlertsViewModel()
    {
        RefreshData();
        
    }
}