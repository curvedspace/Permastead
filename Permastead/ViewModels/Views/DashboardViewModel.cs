using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AIMLbot.AIMLTagHandlers;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Models;
using SkiaSharp;

namespace Permastead.ViewModels.Views;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty] private ScoreBoard _scoreBoard;

    public IEnumerable<ISeries> Series { get; set; } 

    public IEnumerable<ISeries> PlantBreakdownSeries { get; set; } 
    
    public IEnumerable<ISeries> PlantingSuccessSeries { get; set; } 
    
    [ObservableProperty] 
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty] private long _totalPlantings;
    [ObservableProperty] private long _successfulPlantings;
    [ObservableProperty] private long _deadPlantings;
    [ObservableProperty] private long _totalHarvestedPlants;
    
    private DateTime PlantingYearStartDate;
        
    private DateTime PlantingYearEndDate;

    // public LabelVisual Title { get; set; } =
    //     new LabelVisual
    //     {
    //         Text = "My chart title",
    //         TextSize = 25,
    //         Padding = new LiveChartsCore.Drawing.Padding(15),
    //         Paint = new SolidColorPaint(SKColors.DarkSlateGray)
    //     };
    
    public DashboardViewModel()
    {
        ScoreBoard = AppSession.Instance.CurrentScoreboard;
        
        PlantingYearStartDate = new DateTime(DateTime.Today.Year, 1,1);
        PlantingYearEndDate = new DateTime(DateTime.Today.Year, 12,31);
        
        // get other data
        Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantings(AppSession.ServiceMode));
        
        //compute the success rate for the current growing year
        foreach (var p in Plantings)
        {
            // if (p.EndDate > PlantingYearStartDate || p.SeedPacket.SeasonalityId == 3)
            {
                TotalPlantings++;

                if (p.State.Code != "DEAD" && p.State.Code != "H")
                {
                    SuccessfulPlantings++;
                }
                
                if (p.State.Code == "DEAD")
                {
                    DeadPlantings++;
                }

                if (p.State.Code == "H")
                {
                    TotalHarvestedPlants++;
                }
            }
        }
        
        Series = 
        GaugeGenerator.BuildSolidGauge(
            new GaugeItem(
                Convert.ToDouble(ScoreBoard.LevelProgress * 100), // the gauge value
                series => // the series style
                {
                    series.MaxRadialColumnWidth = 50;
                    series.DataLabelsSize = 50;
                    series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                }));
        
        PlantBreakdownSeries =
        new[] { 25, 35, 30, 10 }.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });
        
        PlantingSuccessSeries =
            new[] { 15, 20, 65}.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });

      //   var success = new List<double>();
      //   success.Add(Convert.ToDouble(TotalHarvestedPlants/TotalPlantings) * 100);
      //   success.Add(Convert.ToDouble(DeadPlantings/TotalPlantings) * 100);
      //   success.Add(Convert.ToDouble((TotalPlantings - TotalHarvestedPlants - DeadPlantings) / TotalPlantings) * 100);
      //
      // PlantingSuccessSeries = success.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });
        
        // var breakdown = new List<int>();
        // breakdown.Add(Convert.ToInt32(TotalHarvestedPlants));
        // breakdown.Add(Convert.ToInt32(SuccessfulPlantings));
        // breakdown.Add(Convert.ToInt32(TotalPlantings - SuccessfulPlantings));
        //
        // PlantBreakdownSeries
        //     = breakdown.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });
    }
}