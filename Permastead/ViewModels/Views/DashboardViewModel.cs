using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Permastead.ViewModels.Views;

public class DashboardViewModel : ViewModelBase
{

    public ISeries[] Series { get; set; } 
        = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };
    
    public DashboardViewModel()
    {
        
    }

    
}