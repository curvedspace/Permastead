using Models;

namespace Models;

public class LandPlot : CodeTable
{
    public Location? Location { get; set; }

    public int PermaCultureZone { get; set; }

    public string? HardinessZone { get; set; }
    
}