using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class ProceduresWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private StandardOperatingProcedure _currentItem;

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
}