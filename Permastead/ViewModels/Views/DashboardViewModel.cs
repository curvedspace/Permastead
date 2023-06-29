
using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Core;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

    public partial class DashboardViewModel : DockBase
    {
        public QuoteViewModel QuoteViewModel { get; set; } = new QuoteViewModel();

        [ObservableProperty] 
        private ObservableCollection<Observation> _observations = new ObservableCollection<Observation>();
        
        [ObservableProperty] 
        private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
        
        [ObservableProperty] 
        private ObservableCollection<Inventory> _inventoryItems = new ObservableCollection<Inventory>();
        
        [ObservableProperty]
        private string? _statistics;
        
        [ObservableProperty]
        private double _totalScore;
        
        public string TotalScoreFormatted => _totalScore.ToString("F1");
        
        [ObservableProperty]
        private double _totalScoreNormalized;
        
        [ObservableProperty]
        private decimal _observationsToActionRatio;
        
        public DashboardViewModel()
        {
            this.QuoteViewModel.Quote.Description = "This is a test quote";
            this.QuoteViewModel.Quote.AuthorName = "Anonymous";
            
            //get observations
            Observations = new ObservableCollection<Observation>(Services.ObservationsService.GetObservations(AppSession.ServiceMode));
            
            var scoreBoard = ScoreBoardService.ComputeTotalScore(ServiceMode.Local);

            this.Statistics = scoreBoard.ToString();
            this.TotalScore = (double)scoreBoard.TotalScore;

            // this should represent how close, in a scale of 1-100, that we are to the next level
            TotalScoreNormalized = Math.Round((this._totalScore - scoreBoard.LevelMin) / (scoreBoard.LevelMax - scoreBoard.LevelMin),4);

            decimal ratio = 0;

            if (scoreBoard.Actions > 0)
            {
                //ratio = (scoreBoard.Observations - scoreBoard.Actions) / scoreBoard.Observations;
                ratio = (scoreBoard.Observations / scoreBoard.Actions);
            }
            
            ObservationsToActionRatio = ratio;
            
            // get other data
            Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantings(AppSession.ServiceMode));
            InventoryItems = new ObservableCollection<Inventory>(Services.InventoryService.GetAllInventory(AppSession.ServiceMode));
            
        }

        public void GetQuote()
        {
            this.QuoteViewModel.Quote = Services.QuoteService.GetRandomQuote(AppSession.ServiceMode);
        }

    }

