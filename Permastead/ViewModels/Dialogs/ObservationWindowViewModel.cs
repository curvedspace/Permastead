using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;
using Serilog.Context;

namespace Permastead.ViewModels.Dialogs;

public partial class ObservationWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Observation _observation;
    
    [ObservableProperty]
    private ObservableCollection<CommentType> _commentTypes = new ObservableCollection<CommentType>();
    
    public ObservationWindowViewModel(Observation obs)
    {
        _observation = obs;
        
        CommentTypes =
            new ObservableCollection<CommentType>(
                Services.ObservationsService.GetCommentTypes(AppSession.ServiceMode));
        _observation.CommentType = CommentTypes.First(x => x.Id == _observation.CommentType.Id);
        
        OnPropertyChanged(nameof(_observation));
    }
    
    public ObservationWindowViewModel()
    {
        _observation = new Observation();
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
        }
    }
}