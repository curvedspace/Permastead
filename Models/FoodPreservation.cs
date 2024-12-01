namespace Models;

public class FoodPreservation
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public Harvest? Harvest { get; set; }
    
    public FoodPreservationType? Type { get; set; }
    
    public long Measurement { get; set; }
    
    public MeasurementUnit Units { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public string StartDateString => this.StartDate.ToShortDateString();
    
    public DateTime EndDate { get; set; }
    
    public string EndDateString => this.EndDate.ToShortDateString();

    public DateTime LastUpdatedDate { get; set; }
    
    public string LastUpdatedDateString => this.LastUpdatedDate.ToShortDateString();

    public FoodPreservation()
    {
        this.CreationDate = DateTime.Now;
        this.Units = new MeasurementUnit();
        this.Type = new FoodPreservationType();
    }
    
}