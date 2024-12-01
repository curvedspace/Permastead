namespace Models;

public class Harvest
{
    public long Id { get; set; }

    public string? Description { get; set; }
    
    public HarvestType? Type { get; set; }
    
    public long HarvestTypeId
    {
        get
        {
            var harvestType = this.Type;
            if (harvestType != null) 
                return harvestType.Id;
            else
            {
                return 0;
            }
        }
    }
    
    public long HarvestAuxId { get; set; }

    public long Measurement { get; set; }
    
    public MeasurementUnit Units { get; set; }
    
    public DateTime CreationDate { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public string Comment { get; set; }

    public Harvest()
    {
        this.CreationDate = DateTime.Now;
        this.Type = new HarvestType();
        this.HarvestAuxId = 0;
        this.Units = new MeasurementUnit();
    }
}