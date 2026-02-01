using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;
using Serilog.Context;
using Services;
using Ursa.Controls;

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
    
    //message box data
    private readonly string _shortMessage = "Are you sure you want to delete this procedure?";
    private string _message;
    private string? _title = "Deletion Confirmation";
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }
    
    public ICommand YesNoCommand { get; set; }
    
    public WindowToastManager? ToastManager { get; set; }

    [RelayCommand]
    private async Task DeleteProcedure()
    {
        try
        {
            if (CurrentItem != null)
            {
                await OnYesNoAsync();

                if (Result == MessageBoxResult.Yes)
                {
                    bool rtnValue = Services.ProceduresService.DeleteRecord(AppSession.ServiceMode, CurrentItem);
                    ToastManager?.Show(new Toast("Procedure record (" + CurrentItem.Name + ") has been removed."));

                    OnPropertyChanged(nameof(CurrentItem));

                    using (LogContext.PushProperty("ProceduresViewModel", this))
                    {
                        if (CurrentItem.Id == 0) CurrentItem.Author = AppSession.Instance.CurrentUser;
                        Log.Information("Removed procedure: " + CurrentItem.Name, rtnValue);
                    }

                    RefreshDataOnly();
                }
            }
        }
        catch (Exception e)
        {
        }
    }

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
        CurrentItem = new StandardOperatingProcedure();
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
            ToastManager?.Show(new Toast("Procedure (" + CurrentItem.Name + ") has been updated."));

            var savedItem = CurrentItem;
            RefreshData();
            CurrentItem = savedItem;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public ProceduresViewModel()
    {
        RefreshDataOnly();
        ClearSearch();
        
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
            
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.Question;
        _message = _shortMessage;
    }
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task Show(MessageBoxButton button)
    {
        Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
    }
}