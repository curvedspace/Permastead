using System.ComponentModel.DataAnnotations;

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
    
    public PlantingState State { get; set; }
    
    public long PlantingStateId => this.State!.Id;

    public string Comment { get; set; } 

    public Person Author { get; set; }

    public long AuthorId => this.Author!.Id;

    public string StartDateString => this.StartDate.ToShortDateString();

    public string Age
    {
        get
        {
            string calculatedAge = "Unavailable.";

            if (DateTime.Now < this.StartDate)
            {
                calculatedAge = "Not yet planted.";
            }
            else
            {
                
                if ((DateTime.Now - this.StartDate).Days >= 365)
                {
                    calculatedAge = (DateTime.Now - this.StartDate).Days + " day(s).";
                }
                else
                {
                    calculatedAge = ((DateTime.Now - this.StartDate).Days / 365).ToString("0.##") + " year(s).";
                }
            }

            return calculatedAge;
        }
    }
    

    public Planting()
    {
        this.Code = string.Empty;
        this.Description = string.Empty;
        this.Plant = new Plant();
        this.SeedPacket = new SeedPacket();
        this.State = new PlantingState();
        this.Author = Person.Anonymous();
        this.Bed = new GardenBed();
        this.Comment = string.Empty;
    }

}