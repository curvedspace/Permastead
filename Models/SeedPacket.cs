
namespace Models;

/// <summary>
/// Models a packet of seeds, part of a Planting class.
/// </summary>
public class SeedPacket
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public bool IsPlanted { get; set; } = false;

    public int DaysToHarvest { get; set; }

    public Plant? Plant { get; set; }

    public long PlantId => this.Plant?.Id ?? 0;

    public Vendor Vendor { get; set; }

    public long VendorId => this.Vendor?.Id ?? 0;

    public Person? Author { get; set; }

    public long AuthorId => this.Author?.Id ?? 0;

    public DateTimeOffset BestByDate { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.MaxValue;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates is the item is currently active, meaning it's start date is earlier than today
    /// and it's end date is in the future.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrent()
    {
        if ((DateTime.UtcNow > StartDate) && (DateTime.UtcNow < EndDate))
            return true;
        else
            return false;
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
        this.Author = Person.Anonymous();

        this.StartDate = DateTime.UtcNow;
        this.EndDate = new DateTime(2100, 12, 31);
        this.BestByDate = this.StartDate.AddYears(5);
    }
}