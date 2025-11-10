using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;

namespace Permastead.ViewModels.Views;

public partial class  CyclesViewModel : ViewModelBase
{
    public double Value { get; set; } = 30;
    
    public IEnumerable<ISeries> Series { get; set; }
    
    [RelayCommand]
    private void RefreshData()
    {
        Series = GaugeGenerator.BuildSolidGauge();
    }

    public CyclesViewModel()
    {
        RefreshData();
    }
}