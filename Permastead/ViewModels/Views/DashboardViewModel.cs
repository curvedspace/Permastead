using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Services;
using SkiaSharp;

namespace Permastead.ViewModels.Views;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty] private ScoreBoard _scoreBoard;
    
    [ObservableProperty] 
    private double[] _actSeries;
    
    [ObservableProperty] 
    private double[] _obsSeries;
    
    [ObservableProperty] 
    private double[] _pltSeries;
    
    [ObservableProperty] 
    private double[] _hrvSeries;

    public IEnumerable<ISeries> Series { get; set; } 

    public IEnumerable<ISeries> PlantBreakdownSeries { get; set; } 
    
    public IEnumerable<ISeries> PlantingSuccessSeries { get; set; } 
    
    public ISeries[] ActObsSeries { get; set; }
    
    
    public Func<double, string> MonthFormatter { get; set; } =
        date => new DateTime(1,(int)date+1,1).ToString("MMMM");
    
    public decimal LevelProgress {get => _scoreBoard.LevelProgress *  100;}
    
    public SolidColorPaint LegendTextPaint { get; set; } =
        new SolidColorPaint()
        {
            Color = new SKColor(240, 240, 240)
        };

    [ObservableProperty] public IList<string> _chartLabels;
    
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August","September","October","November","December" },
            LabelsRotation = 0,
            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            SeparatorsAtCenter = false,
            TicksPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            TicksAtCenter = true,
            // By default the axis tries to optimize the number of 
            // labels to fit the available space, 
            // when you need to force the axis to show all the labels then you must: 
            ForceStepToMin = true, 
            MinStep = 1 
        }
    };
    
    [ObservableProperty] private Observation _yearInReview;
    
    [ObservableProperty] 
    private ObservableCollection<Planting> _plantings = new ObservableCollection<Planting>();
    
    [ObservableProperty] private string _plantingYear;
    
    [ObservableProperty] 
    private ObservableCollection<string> _plantingYears = new ObservableCollection<string>();
    
    [ObservableProperty] private long _totalPlantings;
    [ObservableProperty] private long _successfulPlantings;
    [ObservableProperty] private long _deadPlantings;
    [ObservableProperty] private long _totalHarvestedPlants;
    [ObservableProperty] private long _totalActivePlantings;
    [ObservableProperty] private long _totalYearPlantings;
    
    
    public ObservableValue SuccessfulPlantingsValue { get; set; } = new ObservableValue(0);
    public ObservableValue DeadPlantingsValue { get; set; } = new ObservableValue(0);
    public ObservableValue HarvestedPlantingsValue { get; set; } = new ObservableValue(0);
   
    public ObservableValue AnnualsValue { get; set; } = new ObservableValue(0);
    public ObservableValue BiennialsValue { get; set; } = new ObservableValue(0);
    public ObservableValue PerennialsValue { get; set; } = new ObservableValue(0);
    
    private DateTime _plantingYearStartDate;
        
    private DateTime _plantingYearEndDate;
    
    
    [RelayCommand]
    private void RefreshData()
    {
        ScoreBoard = ScoreBoardService.ComputeTotalScore(AppSession.ServiceMode);
        AppSession.Instance.CurrentScoreboard = ScoreBoard;
        
        RefreshDataOnly();

    }

    [RelayCommand]
    private void SaveYearInReviewComment()
    {
        ObservationsService.CommitRecord(AppSession.ServiceMode, YearInReview);
    }
    
    public void RefreshDataOnly()
    {
        Console.WriteLine("Refreshing Dashboard data for year " + PlantingYear);

        if (PlantingYear == "ALL")
        {
            _plantingYearStartDate = new DateTime(1970, 1,1);
            _plantingYearEndDate = new DateTime(2100, 12,31);
        }
        else
        {
            _plantingYearStartDate = new DateTime(Convert.ToInt32(PlantingYear), 1,1);
            _plantingYearEndDate = new DateTime(Convert.ToInt32(PlantingYear), 12,31);
        }

        if (PlantingYear == "ALL")
        {
            YearInReview = new Observation();
        }
        else
        {
            YearInReview = ObservationsService.GetCurrentYearInReview(AppSession.ServiceMode, Convert.ToInt32(PlantingYear));
        }
        
        YearInReview.StartDate = _plantingYearStartDate;
        YearInReview.EndDate = _plantingYearEndDate;
        
        // reset counters
        SuccessfulPlantings = 0;
        DeadPlantings = 0;
        TotalHarvestedPlants = 0;

        TotalActivePlantings = 0;
        TotalYearPlantings = 0;
            
        // get other data
        Plantings = new ObservableCollection<Planting>(Services.PlantingsService.GetPlantings(AppSession.ServiceMode));
        
        //compute the success rate for the current growing year
        foreach (var p in Plantings)
        {
            if (PlantingYear == "ALL")
            {
                TotalYearPlantings += 1;
                
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
            else
            {
                if (p.StartDate.Year == _plantingYearStartDate.Year || p.EndDate >= _plantingYearEndDate )
                {
                    if (p.StartDate.Year == _plantingYearStartDate.Year)
                    {
                        TotalYearPlantings++;
                        TotalPlantings++;
                    }

                    if (p.State.Code != "DEAD" && p.State.Code != "H" && p.StartDate.Year == _plantingYearStartDate.Year)
                    {
                        SuccessfulPlantings++;
                    }
                    
                    if (p.State.Code == "DEAD" && p.EndDate.Year == _plantingYearEndDate.Year)
                    {
                        DeadPlantings++;
                    }

                    if (p.State.Code == "H" && p.EndDate.Year == _plantingYearEndDate.Year)
                    {
                        TotalHarvestedPlants++;
                    }
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
        
        
      //get plant breakdown
        Int32 annuals = 0;
        Int32 biennials = 0;
        Int32 perennials = 0;
      
        foreach (var p in Plantings)
        {
            if (PlantingYear == "ALL")
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
            else
            {
                if (p.EndDate >= _plantingYearEndDate && p.StartDate.Year == _plantingYearStartDate.Year)
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
            
        }
      
        var breakdown = new List<int>();
        breakdown.Add(annuals);
        breakdown.Add(biennials);
        breakdown.Add(perennials);
        
        
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
            
        PlantingSuccessSeries =
            GaugeGenerator.BuildSolidGauge(
                new GaugeItem(DeadPlantingsValue, series => SetStyle("Deceased", series)),
                new GaugeItem(HarvestedPlantingsValue, series => SetStyle("Harvested", series)),
                new GaugeItem(SuccessfulPlantingsValue, series => SetStyle("Successful", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.InnerRadius = 20;
                }));

        CreateObsActChart();
        
        Console.WriteLine("Total plantings for year " + TotalYearPlantings);
        Console.WriteLine("");
    }
    
    public DashboardViewModel()
    {
        
        ScoreBoard = AppSession.Instance.CurrentScoreboard;
        PlantingYear = DateTime.Now.Year.ToString(CultureInfo.CurrentCulture);
        YearInReview = ObservationsService.GetCurrentYearInReview(AppSession.ServiceMode, Convert.ToInt32(PlantingYear));
        
        //setup dropdown to cover all the years we have data for
        //get earliest record
        int firstYear = ScoreBoardService.GetEarliestRecordYear(AppSession.ServiceMode);

        PlantingYears.Clear();
        
        if (firstYear != DateTime.Today.Year)
        {
            for (int year = firstYear; year <= DateTime.Today.Year; year++)
            {
                PlantingYears.Add(year.ToString());
            }
        }
        else
        {
            PlantingYears.Add(firstYear.ToString());
        }
        
        PlantingYears.Add("ALL");
        
        ChartLabels =
        [
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November",
            "December"
        ];

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

    private void CreateObsActChart()
    {
        var actions = Services.ToDoService.GetAllToDos(AppSession.ServiceMode);
        var observations = Services.ObservationsService.GetObservationsForAllEntities(AppSession.ServiceMode);
        var plantings = Services.PlantingsService.GetPlantings(AppSession.ServiceMode);
        var harvests = Services.HarvestService.GetAllHarvests(AppSession.ServiceMode);

        var actionsByMonth = new Dictionary<int, double>();
        var observationsByMonth = new Dictionary<int, double>();
        var plantingsByMonth = new Dictionary<int, double>();
        var harvestsByMonth = new Dictionary<int, double>();
        
        //set up the dictionaries
        for (int i = 1; i <= 12; i++)
        {
            actionsByMonth.Add(i,0);
            observationsByMonth.Add(i,0);
            plantingsByMonth.Add(i,0);
            harvestsByMonth.Add(i,0);
        }
        
        //tally up the actions and observations by month
        foreach (var a in actions)
        {
            if (a.CreationDate >= _plantingYearStartDate && a.CreationDate <= _plantingYearEndDate)
                actionsByMonth[a.DueDate.Date.Month] += 1;
        }
        
        foreach (var o in observations)
        {
            if (o.AsOfDate >= _plantingYearStartDate && o.AsOfDate <= _plantingYearEndDate)
                observationsByMonth[o.AsOfDate.Date.Month] += 1;
        }
        
        foreach (var o in plantings)
        {
            if (o.CreationDate >= _plantingYearStartDate && o.CreationDate <= _plantingYearEndDate)
                plantingsByMonth[o.CreationDate.Date.Month] += 1;
        }
        
        foreach (var o in harvests)
        {
            if (o.CreationDate >= _plantingYearStartDate && o.CreationDate <= _plantingYearEndDate)
                harvestsByMonth[o.CreationDate.Date.Month] += 1;
        }

        
        var actSeries = new ColumnSeries<double>();
        actSeries.Name = "Actions";
        actSeries.Values = actionsByMonth.Values;

        
        
        var obsSeries = new ColumnSeries<double>();
        obsSeries.Name = "Observations";
        obsSeries.Values = observationsByMonth.Values;
        
        var pltSeries = new ColumnSeries<double>();
        pltSeries.Name = "Plantings";
        pltSeries.Values = plantingsByMonth.Values;
        
        var hrvSeries = new ColumnSeries<double>();
        hrvSeries.Name = "Harvests";
        hrvSeries.Values = harvestsByMonth.Values;

        ActObsSeries = new[] { actSeries, obsSeries, pltSeries, hrvSeries };
        
        ActSeries = actionsByMonth.Values.ToArray();
        ObsSeries = observationsByMonth.Values.ToArray();
        PltSeries = plantingsByMonth.Values.ToArray();
        HrvSeries = harvestsByMonth.Values.ToArray();
        
        OnPropertyChanged(nameof(ActObsSeries));
    }
}