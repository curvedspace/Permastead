using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace Permastead.ViewModels.Views;

public partial class DashboardViewModel : ViewModelBase
{
    public IEnumerable<ISeries> Series { get; set; } =
        GaugeGenerator.BuildSolidGauge(
            new GaugeItem(
                30,          // the gauge value
                series =>    // the series style
                {
                    series.MaxRadialColumnWidth = 50;
                    series.DataLabelsSize = 50;
                }));
    
    public IEnumerable<ISeries> PlantSeries { get; set; } =
        new[] { 25,35,30,10}.AsPieSeries((value, series) =>
        {
            series.MaxRadialColumnWidth = 60;
        });
}