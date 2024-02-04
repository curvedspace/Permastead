using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AIMLbot.AIMLTagHandlers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using LiveChartsCore;
using LiveChartsCore.Defaults;
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
    
    [ObservableProperty] private int _plantingYear;
    
    [ObservableProperty] 
    private ObservableCollection<int> _plantingYears = new ObservableCollection<int>();
    
    [ObservableProperty] private long _totalPlantings;
    [ObservableProperty] private long _successfulPlantings;
    [ObservableProperty] private long _deadPlantings;
    [ObservableProperty] private long _totalHarvestedPlants;
    [ObservableProperty] private long _totalActivePlantings;

    // [ObservableProperty] private long _totalPerennials;
    // [ObservableProperty] private long _totalBiennials;
    // [ObservableProperty] private long _totalAnnuals;
    
    public ObservableValue SuccessfulPlantingsValue { get; set; } = new ObservableValue(0);
    public ObservableValue DeadPlantingsValue { get; set; } = new ObservableValue(0);
    public ObservableValue HarvestedPlantingsValue { get; set; } = new ObservableValue(0);
    
    public ObservableValue AnnualsValue { get; set; } = new ObservableValue(0);
    public ObservableValue BiennialsValue { get; set; } = new ObservableValue(0);
    public ObservableValue PerennialsValue { get; set; } = new ObservableValue(0);
    
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

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }

    public void RefreshDataOnly()
    {
        Console.WriteLine("Refreshing Dashboard data for year " + PlantingYear);
        
        PlantingYearStartDate = new DateTime(PlantingYear, 1,1);
        PlantingYearEndDate = new DateTime(PlantingYear, 12,31);
        
        // reset counters
        SuccessfulPlantings = 0;
        DeadPlantings = 0;
        TotalHarvestedPlants = 0;

        TotalActivePlantings = 0;
            
        // get other data
        Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantings(AppSession.ServiceMode));
        
        //compute the success rate for the current growing year
        foreach (var p in Plantings)
        {
            if (p.EndDate >= PlantingYearEndDate || p.StartDate >= PlantingYearStartDate)
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
        
        // PlantBreakdownSeries =
        // new[] { 25, 35, 30, 10 }.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });
        //
        // PlantingSuccessSeries =
        //     new[] { 15, 20, 65}.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });

      //   var success = new List<double>();
      //   success.Add(Convert.ToDouble(TotalHarvestedPlants/TotalPlantings) * 100);
      //   success.Add(Convert.ToDouble(DeadPlantings/TotalPlantings) * 100);
      //   success.Add(Convert.ToDouble((TotalPlantings - TotalHarvestedPlants - DeadPlantings) / TotalPlantings) * 100);
      //
      // PlantingSuccessSeries = success.AsPieSeries((value, series) => { series.MaxRadialColumnWidth = 60; });
        
      //get plant breakdown
        Int32 annuals = 0;
        Int32 biennials = 0;
        Int32 perennials = 0;
      
        foreach (var p in Plantings)
        {
            if (p.EndDate >= PlantingYearEndDate || p.StartDate >= PlantingYearStartDate)
            {
                switch (p.SeedPacket.Seasonality.Code)
                {
                    case "A":
                        annuals++;
                        break;
                    case "B":
                        biennials++;
                        break;
                    case "P":
                        perennials++;
                        break;
                }

                TotalActivePlantings++;
            }
        }
      
        var breakdown = new List<int>();
        breakdown.Add(annuals);
        breakdown.Add(biennials);
        breakdown.Add(perennials);
        
        
        // PerennialsValue = new ObservableValue(0);
        // BiennialsValue = new ObservableValue(0);
        // AnnualsValue = new ObservableValue(0);

        PerennialsValue.Value = perennials;
        BiennialsValue.Value = biennials;
        AnnualsValue.Value = annuals;
        
        PlantBreakdownSeries =
            GaugeGenerator.BuildSolidGauge(
                new GaugeItem(AnnualsValue, series => SetStyle("Annuals", series)),
                new GaugeItem(BiennialsValue, series => SetStyle("Biennials", series)),
                new GaugeItem(PerennialsValue, series => SetStyle("Perennials", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.InnerRadius = 20;
                }));

        // values need to be observable for them to update properly
        SuccessfulPlantingsValue.Value = SuccessfulPlantings;
        DeadPlantingsValue.Value = DeadPlantings;
        HarvestedPlantingsValue.Value = TotalHarvestedPlants;

        // Console.WriteLine(PlantBreakdownSeries.LongCount());
            
        PlantingSuccessSeries =
            GaugeGenerator.BuildSolidGauge(
                new GaugeItem(SuccessfulPlantingsValue, series => SetStyle("Successful", series)),
                new GaugeItem(DeadPlantingsValue, series => SetStyle("Deceased", series)),
                new GaugeItem(HarvestedPlantingsValue, series => SetStyle("Harvested", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.InnerRadius = 20;
                }));
    }
    
    public DashboardViewModel()
    {
        ScoreBoard = AppSession.Instance.CurrentScoreboard;
        
        PlantingYear = DateTime.Now.Year;
        
        PlantingYears.Clear();
        PlantingYears.Add(2022);
        PlantingYears.Add(2023);
        PlantingYears.Add(2024);
        
        RefreshDataOnly();
        
    }
    
    private static void SetStyle(string name, PieSeries<ObservableValue> series)
    {
        series.Name = name;
        series.DataLabelsPosition = PolarLabelsPosition.Start;
        series.DataLabelsFormatter =
            point => $"{point.Coordinate.PrimaryValue} {point.Context.Series.Name}";
        series.InnerRadius = 20;
        series.RelativeOuterRadius = 8;
        series.RelativeInnerRadius = 8;
    }
}