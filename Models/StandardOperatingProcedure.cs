namespace Models;

public class StandardOperatingProcedure
{
    public long Id { get; set; }
    
    public DateTime CreationDate { get; set; }

    public DateTime LastUpdatedDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string? Name { get; set; }
    
    public string? Category { get; set; }
    
    public string? Content { get; set; }
    
    public string FullDescription { get => $"{Category}: {Name}"; }
    
    public Person Author { get; set; }
    

    public long AuthorId { get { return this.Author.Id; } }
    
    /// <summary>
    /// Indicates is the item is currently active, meaning it's start date is earlier than today
    /// and it's end date is in the future.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrent()
    {
        return (DateTime.UtcNow > CreationDate) && (DateTime.UtcNow < EndDate);
    }

    public StandardOperatingProcedure()
    {
        CreationDate = DateTime.UtcNow;
        LastUpdatedDate = DateTime.UtcNow;
        Author = Person.Gaia();
        EndDate = DateTime.MaxValue;
    }
    
}