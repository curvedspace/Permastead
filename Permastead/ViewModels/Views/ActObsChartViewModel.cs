using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Permastead.ViewModels.Views;

public class ActObsChartViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; } =
    {
        new ColumnSeries<double>
        {
            Name = "Actions",
            Values = new double[] { 2, 5, 4 }
        },
        new ColumnSeries<double>
        {
            Name = "Observations",
            Values = new double[] { 3, 1, 6 }
        }
    };

    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labels = new string[] { "January", "Category 2", "Category 3" },
            LabelsRotation = 0,
            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            SeparatorsAtCenter = false,
            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            TicksAtCenter = true,
            // By default the axis tries to optimize the number of 
            // labels to fit the available space, 
            // when you need to force the axis to show all the labels then you must: 
            ForceStepToMin = true, 
            MinStep = 1 
        }
    };
}