
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using AIMLbot.AIMLTagHandlers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Serilog;
using Serilog.Context;
using Services;

namespace Permastead.ViewModels.Views;

    public partial class ObservationsViewModel : ViewModelBase
    {

        #region "Variables"
        
        [ObservableProperty]
        private string? _comment; // This is our backing field for Comment
        
        [ObservableProperty]
        private long _wordCount; 
        
        [ObservableProperty]
        private long _observationCount; 

        #endregion
        
        #region "Properties"
        
        public double WordsPerObservation
        {
            get
            {
                if (_observationCount == 0)
                {
                    return 0;
                }
                else
                {
                    return _wordCount / _observationCount;
                }
            }
        }
        
        #endregion
        
        [ObservableProperty] private string _searchText = "";
        
        [ObservableProperty] 
        private ObservableCollection<Observation> _observations = new ObservableCollection<Observation>();
        
        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = "";
            RefreshObservations(SearchText);
        }

        public ObservationsViewModel()
        {
            try
            {
                RefreshObservations();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Error getting observations.");
            }
            
        }
        
        [RelayCommand]
        private void SaveObservation()
        {
            //if there is a comment, save it.

            if (!string.IsNullOrEmpty(_comment))
            {
                var newObservation = new Observation();

                newObservation.AsOfDate = DateTime.Now;
                newObservation.CommentType!.Id = 1;
                newObservation.Author!.Id = AppSession.Instance.CurrentUser.Id;
                newObservation.Comment = _comment;

                var rtnValue = Services.ObservationsService.InsertRecord(AppSession.ServiceMode, newObservation);

                if (rtnValue)
                {
                    _observations.Add(newObservation);
                }

                OnPropertyChanged(nameof(_observations));
                
                using (LogContext.PushProperty("ObservationViewModel", this))
                {
                    Log.Information("Saved observation: ", rtnValue);
                }

                RefreshObservations();

                Comment = string.Empty; //reset comment
                
            }

        }

        [RelayCommand]
        private void RefreshData()
        {
            RefreshObservations();
        }

        public void RefreshObservations(string searchText = "")
        {
            _observations.Clear();
            var obs = Services.ObservationsService.GetObservationsForAllEntities(AppSession.ServiceMode);
            
            var caseAdjustedFilterText = searchText.Trim().ToLowerInvariant();

            //finally, sort by date, add to collection for display
            foreach (var o in obs.OrderByDescending(x=>x.AsOfDate))
            {
                if (string.IsNullOrEmpty(caseAdjustedFilterText))
                {
                    _observations.Add(o);
                }
                else
                {
                    if (o.Comment.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                        o.FullDescription.Contains(caseAdjustedFilterText))
                    {
                        _observations.Add(o);
                    }
                }
            }

            WordCount = ObservationsService.GetObservationWordCount(_observations.ToList());
            ObservationCount = _observations.Count;
            
            OnPropertyChanged(nameof(Observations));
        }
    }

