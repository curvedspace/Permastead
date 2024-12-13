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
    
    public Entity HarvestEntity { get; set; }
    
    public long HarvestAuxId => this.HarvestEntity.Id;

    public long Measurement { get; set; }
    
    public MeasurementUnit Units { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime HarvestDate { get; set; }
    
    public string HarvestDateString => this.HarvestDate.ToShortDateString();

    public DateTime LastUpdatedDate { get; set; }

    public string Comment { get; set; }

    public Harvest()
    {
        this.CreationDate = DateTime.Now;
        this.HarvestDate = DateTime.Today;
        this.Type = new HarvestType();
        this.HarvestEntity = new Entity();
        this.Units = new MeasurementUnit();
    }
}