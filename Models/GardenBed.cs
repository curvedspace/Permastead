namespace Models;

public class GardenBed : CodeTable
{
    public Location Location { get; set; }
    
    public GardenBedType Type { get; set; }
    
    public long PermacultureZone { get; set; }
    
    public Person Author { get; set; }

    public long AuthorId => this.Author!.Id;
    
    public long LocationId => this.Location!.Id;
    
    public long GardenBedTypeId => this.Type!.Id;

    public GardenBed()
    {
        this.Location = new Location();
        this.Location.Id = 1;
        
        this.Type = new GardenBedType();
        this.Author = Person.Gaia();
    }
}