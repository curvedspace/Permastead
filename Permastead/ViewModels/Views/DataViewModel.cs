using System.Collections.Generic;
using System.Data;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using DataAccess.Server;
using Services;

namespace Permastead.ViewModels.Views;

public partial class DataViewModel : ViewModelBase
{
    [ObservableProperty] private DataTable _dataTable;

    [ObservableProperty] private DataGridCollectionView _items;
    
    [ObservableProperty] private List<string> _tables;
    
    [ObservableProperty] private bool _canUserAddRows;
    
    [ObservableProperty] private bool _canUserDeleteRows;
    
    [RelayCommand]
    private void RefreshData()
    {
        DataTable = QuoteRepository.GetAll(DataConnection.GetServerConnectionString());
        Items = new DataGridCollectionView(DataTable.DefaultView);
    }

    public DataViewModel()
    {
        CanUserAddRows = true;
        CanUserDeleteRows = true;
            
        Tables = new List<string>();
        //Tables.Add("Animal Types");
        Tables.Add("Quotes");
        
        DataTable = new DataTable();
        DataTable = QuoteService.GetAllQuotes(AppSession.ServiceMode);
        Items = new DataGridCollectionView(DataTable.DefaultView);
        
    }
    
}