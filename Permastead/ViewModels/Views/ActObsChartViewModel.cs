using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Services;
using SkiaSharp;

namespace Permastead.ViewModels.Views;

public class ActObsChartViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; }

    public SolidColorPaint LegendTextPaint { get; set; } =
        new SolidColorPaint()
        {
            Color = new SKColor(240, 240, 240)
        };

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

    public ActObsChartViewModel()
    {
        var actions = Services.ToDoService.GetAllToDos(AppSession.ServiceMode);
        var observations = Services.ObservationsService.GetObservations(AppSession.ServiceMode);

        var actionsByMonth = new Dictionary<int, double>();
        var observationsByMonth = new Dictionary<int, double>();
        
        //set up the dictionaries
        for (int i = 1; i <= 12; i++)
        {
            actionsByMonth.Add(i,0);
            observationsByMonth.Add(i,0);
        }
        
        //tally up the actions and observations by month
        foreach (var a in actions)
        {
            actionsByMonth[a.DueDate.Date.Month] += 1;
        }
        
        foreach (var o in observations)
        {
            observationsByMonth[o.AsOfDate.Date.Month] += 1;
        }

        var actSeries = new ColumnSeries<double>();
        actSeries.Name = "Actions";
        actSeries.Values = actionsByMonth.Values;
        
        var obsSeries = new ColumnSeries<double>();
        obsSeries.Name = "Observations";
        obsSeries.Values = observationsByMonth.Values;

        Series = new[] { actSeries, obsSeries };
        
    }
}