namespace Models;

public class Harvest
{
    public long Id { get; set; }

    public string? Description { get; set; }
    
    public HarvestType? HarvestType { get; set; }
    
    public long HarvestTypeId
    {
        get
        {
            var harvestType = this.HarvestType;
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
    
    public Person? Author { get; set; }
    
    public long AuthorId 
    { 
        get
        {
            if (Author != null)
            {
                return Author.Id;
            }
            else
            {
                return 0;
            }
        } 
    }

    public string Comment { get; set; }

    public Harvest()
    {
        this.CreationDate = DateTime.Now;
        this.HarvestDate = DateTime.Today;
        this.HarvestType = new HarvestType() {Id = 1, Description = "Plant"};
        this.HarvestEntity = new Entity();
        this.Units = new MeasurementUnit();
    }
}