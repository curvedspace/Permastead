﻿
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
        private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
        
        [ObservableProperty] 
        private ObservableCollection<SeedPacket> _seedPackets = new ObservableCollection<SeedPacket>();
        
        [ObservableProperty]
        private string? _statistics;
        
        [ObservableProperty]
        private double _totalScore;

        public string GrowingSeason => "Growing Season: " + PlantingYearEndDate.Year.ToString();

        public string HravestPlants => "Harvested: " + TotalHarvestedPlants.ToString();
        
        public string TotalScoreFormatted => TotalScore.ToString("F1");
        
        [ObservableProperty]
        private double _totalScoreNormalized;
        
        [ObservableProperty]
        private decimal _observationsToActionRatio;

        private DateTime PlantingYearStartDate;
        
        private DateTime PlantingYearEndDate;

        [ObservableProperty] private long _totalPlantings;
        [ObservableProperty] private long _successfulPlantings;
        [ObservableProperty] private long _totalHarvestedPlants;
        [ObservableProperty] private string _totalPlantStats;
        
        public DashboardViewModel()
        {
            this.QuoteViewModel.Quote.Description = "This is a test quote";
            this.QuoteViewModel.Quote.AuthorName = "Anonymous";
            
            PlantingYearStartDate = new DateTime(DateTime.Today.Year, 1,1);
            PlantingYearEndDate = new DateTime(DateTime.Today.Year, 12,31);
            
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
                ratio = (scoreBoard.Observations / scoreBoard.Actions);
            }
            
            ObservationsToActionRatio = ratio;
            
            // get other data
            Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantings(AppSession.ServiceMode));
            InventoryItems = new ObservableCollection<Inventory>(Services.InventoryService.GetAllInventory(AppSession.ServiceMode));
            SeedPackets = new ObservableCollection<SeedPacket>(PlantingsService.GetSeedPackets(AppSession.ServiceMode));
            Plants = new ObservableCollection<Plant>(PlantingsService.GetPlants(AppSession.ServiceMode));

            //compute the success rate for the current growing year
            foreach (var p in Plantings)
            {
                if (p.EndDate > PlantingYearStartDate)
                {
                    TotalPlantings++;

                    if (p.State.Code != "DEAD")
                    {
                        SuccessfulPlantings++;
                    }

                    if (p.State.Code == "H")
                    {
                        TotalHarvestedPlants++;
                    }
                }
            }

            TotalPlantStats = "Total: " + TotalPlantings + " Harvested: " + TotalHarvestedPlants + " Failed: " +
                              (TotalPlantings - SuccessfulPlantings);
        }

        public void GetQuote()
        {
            this.QuoteViewModel.Quote = Services.QuoteService.GetRandomQuote(AppSession.ServiceMode);
        }

    }

