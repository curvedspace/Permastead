namespace Models;

public class FoodPreservation
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public FoodPreservationType? PreservationType { get; set; }
    
    public long PreservationTypeId 
    { 
        get
        {
            if (PreservationType != null)
            {
                return PreservationType.Id;
            }
            else
            {
                return 0;
            }
        } 
    }
    
    public Harvest Harvest { get; set; }
    
    public long HarvestId => this.Harvest.Id;
    
    public long Rating { get; set; }
    
    public long Measurement { get; set; }
    
    public MeasurementUnit Units { get; set; }
    
    public long MeasurementTypeId => this.Units.Id;
    
    public string Comment { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public string StartDateString => this.StartDate.ToShortDateString();
    
    public DateTime EndDate { get; set; }
    
    public string EndDateString => this.EndDate.ToShortDateString();

    public DateTime LastUpdatedDate { get; set; }
    
    public string LastUpdatedDateString => this.LastUpdatedDate.ToShortDateString();
    
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

    public FoodPreservation()
    {
        this.CreationDate = DateTime.Now;
        this.Units = new MeasurementUnit();
        this.PreservationType = new FoodPreservationType();
        this.Author = Person.Anonymous();
        this.Harvest = new Harvest();
    }
    
}