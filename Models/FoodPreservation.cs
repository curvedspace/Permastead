namespace Models;

public class FoodPreservation
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public FoodPreservationType? Type { get; set; }
    
    public Entity PreservationEntity { get; set; }
    
    public long PreservationEntityId => this.PreservationEntity.Id;
    
    public long Rating { get; set; }
    public long Measurement { get; set; }
    
    public MeasurementUnit Units { get; set; }
    
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
        this.Type = new FoodPreservationType();
        this.Author = Person.Anonymous();
        this.PreservationEntity = new Entity();
    }
    
}