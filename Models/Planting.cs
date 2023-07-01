namespace Models;

public class Planting: CodeTable
{
    public Plant Plant { get; set; }
    
    public long PlantId => this.Plant!.Id;
    
    public SeedPacket SeedPacket { get; set; }
    
    public long SeedPacketId => this.SeedPacket!.Id;
    
    public GardenBed Bed { get; set; }
    
    public long BedId => this.Bed!.Id;

    public decimal YieldRating { get; set; } = 0;

    public string Comment { get; set; } 

    public Person Author { get; set; }

    public long AuthorId => this.Author!.Id;

    public string StartDateString => this.StartDate.ToShortDateString();
    

    public Planting()
    {
        this.Code = string.Empty;
        this.Description = string.Empty;
        this.Plant = new Plant();
        this.SeedPacket = new SeedPacket();
        this.Author = Person.Anonymous();
        this.Bed = new GardenBed();
        this.Comment = string.Empty;
    }

}