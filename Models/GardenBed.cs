namespace Models;

public class GardenBed : CodeTable
{
    public Location Location { get; set; }
    
    public GardenBedType Type { get; set; }
    
    public long PermaCultureZone { get; set; }
    
    public Person Author { get; set; }

    public long AuthorId => this.Author!.Id;

    public GardenBed()
    {
        this.Location = new Location();
        this.Type = new GardenBedType();
        this.Author = Person.Anonymous();
    }
}