using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class ObservationWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Observation _observation;
    
    [ObservableProperty]
    private ObservableCollection<CommentType> _commentTypes = new ObservableCollection<CommentType>();
    
    private ObservationsViewModel _controlViewModel { get; set;  } = new ObservationsViewModel();
    
    public ObservationWindowViewModel(Observation obs, ObservationsViewModel obsVm)
    {
        _observation = obs;
        _controlViewModel = obsVm;
        
        CommentTypes =
            new ObservableCollection<CommentType>(
                Services.ObservationsService.GetCommentTypes(AppSession.ServiceMode));
        _observation.CommentType = CommentTypes.First(x => x.Id == _observation.CommentType.Id);
        
        OnPropertyChanged(nameof(_observation));
        
    }
    
    public ObservationWindowViewModel()
    {
        _observation = new Observation();
        
        CommentTypes =
            new ObservableCollection<CommentType>(
                Services.ObservationsService.GetCommentTypes(AppSession.ServiceMode));
        // _observation.CommentType = CommentTypes.First(x => x.Id == _observation.CommentType.Id);
        
        OnPropertyChanged(nameof(_observation));
    }
    
    public void SaveObservation()
    {
        bool rtnValue;
        
        //if there is a comment, save it.
        if (!string.IsNullOrEmpty(_observation.Comment))
        {
            rtnValue = Services.ObservationsService.CommitRecord(AppSession.ServiceMode, _observation);
            
            OnPropertyChanged(nameof(_observation));
                
            using (LogContext.PushProperty("ObservationViewModel", this))
            {
                Log.Information("Saved observation: ", rtnValue);
            }
            
            _controlViewModel.RefreshObservations();
        }
    }
}