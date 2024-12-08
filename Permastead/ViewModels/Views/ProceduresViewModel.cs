using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class  ProceduresViewModel : ViewModelBase
{
 [ObservableProperty]
    private ObservableCollection<StandardOperatingProcedure> _procedures = new ObservableCollection<StandardOperatingProcedure>();
    
    [ObservableProperty] 
    private bool _currentOnly = true;

    [ObservableProperty]
    private long _procedureCount;

    [ObservableProperty]
    private StandardOperatingProcedure _currentItem;
    
    [ObservableProperty]
    private string _searchText = "";
    
   
    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }
    
    [RelayCommand]
    public void RefreshDataOnly(string filterText = "")
    {
        Procedures.Clear();
        
        var procedures = Services.ProceduresService.GetAllProcedures(AppSession.ServiceMode);
        
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();
        
        foreach (var proc in procedures)
        {
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                if (CurrentOnly)
                {
                    if (proc.IsCurrent()) Procedures.Add(proc);
                }
                else
                {
                    Procedures.Add(proc);
                }
            }
            else
            {
                if (proc.Name.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    proc.Category.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    proc.Content.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    if (CurrentOnly)
                    {
                        if (proc.IsCurrent()) Procedures.Add(proc);
                    }
                    else
                    {
                        Procedures.Add(proc);
                    }
                }
            }
        }
        
        if (Procedures.Count > 0) 
            CurrentItem = Procedures.FirstOrDefault()!;
        else
            CurrentItem = new StandardOperatingProcedure();

        ProcedureCount = Procedures.Count;
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
    }

    [RelayCommand]
    private void UpdateRecord()
    {
        try
        {
            //saves the SOP to database
            CurrentItem.Author!.Id = AppSession.Instance.CurrentUser.Id;
            CurrentItem.LastUpdatedDate = DateTime.Now;
            
            ProceduresService.CommitRecord(AppSession.ServiceMode, CurrentItem);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public ProceduresViewModel()
    {
        RefreshDataOnly();
    }
}