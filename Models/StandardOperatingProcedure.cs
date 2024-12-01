namespace Models;

public class StandardOperatingProcedure
{
    public long Id { get; set; }
    
    public DateTime CreationDate { get; set; }

    public DateTime LastUpdatedDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string Content { get; set; }
    
}