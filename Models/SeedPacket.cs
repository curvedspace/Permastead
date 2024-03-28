
namespace Models;

/// <summary>
/// Models a packet of seeds, part of a Planting class.
/// </summary>
public class SeedPacket
{
    public long Id { get; set; }

    public string? Code { get; set; }
    
    public string? Description { get; set; }
    
    public string? Species { get; set; }

    public string? Instructions { get; set; }

    public bool IsPlanted { get; set; } = false;

    public int DaysToHarvest { get; set; }

    public Seasonality? Seasonality { get; set; }
    
    public long SeasonalityId => this.Seasonality?.Id ?? 0;
    
    public Plant? Plant { get; set; }

    public long PlantId => this.Plant?.Id ?? 0;

    public Vendor Vendor { get; set; }

    public long VendorId => this.Vendor?.Id ?? 0;

    public Person? Author { get; set; }

    public long AuthorId => this.Author?.Id ?? 0;

    public DateTime BestByDate => this.StartDate.AddYears(5);
    
    public long Generations { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.MaxValue;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public bool Exchange { get; set; }
    
    public long PacketCount { get; set; } = 1;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        SeedPacket? sp = obj as SeedPacket;
        if (sp == null)
            return false;
        else
            return (sp.Id == Id) && (sp.Code == Code) && (sp.Description == Description);

    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Code, Description);
    }
    
    public bool Equals(SeedPacket? obj)
    {
        return obj != null && (obj.Id == Id) && (obj.Code == Code) && (obj.Description == Description);
    }
    
    /// <summary>
    /// Indicates is the item is currently active, meaning it's start date is earlier than today
    /// and it's end date is in the future.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrent()
    {
        return (DateTime.UtcNow > StartDate) && (DateTime.UtcNow < EndDate);
    }

    public bool IsInThePast()
    {
        return DateTime.UtcNow > StartDate;
    }

    public bool IsInTheFuture()
    {
        return !IsInThePast();
    }

    public SeedPacket()
    {
        this.Plant = new Plant();
        this.Vendor = new Vendor();
        this.Author = Person.Gaia();
        this.Seasonality = new Seasonality();

        this.StartDate = DateTime.UtcNow;
        this.EndDate = new DateTime(2100, 12, 31);

        this.Exchange = false;
    }
}