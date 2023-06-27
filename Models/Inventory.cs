namespace Models;

public class Inventory 
{
    public long Id { get; set; }

    public string Description { get; set; } = String.Empty;
    
    public InventoryGroup InventoryGroup { get; set; }
    
    public long InventoryGroupId { get { return this.InventoryGroup.Id; } }
    
    public InventoryType InventoryType { get; set; }
    
    public long InventoryTypeId { get { return this.InventoryType.Id; } }
    
    public double OriginalValue { get; set; }
    
    public double CurrentValue { get; set; }
    
    public long Quantity { get; set; }
    
    public bool ForSale { get; set; }

    public string Room { get; set; }

    public string Brand { get; set; }
    
    public string Notes { get; set; }
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public Person? Author { get; set; }
    
    public long AuthorId 
    { 
        get
        {
            if (Author != null) return Author.Id;
        } 
    }

    public string StartDateString => this.StartDate.ToShortDateString();

    public bool IsActive { get; set; }

    public bool IsTransient { get
    {
        if ((Id == 0)) return true;
        return false;
    } }

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

    public Inventory()
    {
        this.StartDate = DateTime.Today;
        this.CreationDate = DateTime.Now;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Anonymous();
        this.InventoryGroup = new InventoryGroup();
        this.InventoryType = new InventoryType();
        this.Quantity = 1;
        this.ForSale = false;
        this.Brand = "Unknown";
        this.Notes = "";
        this.Room = "N/A";
    }
        
    public Inventory(long id) : this()
    {
        this.Id = id;
    }
    
    public override string ToString()
    {
        return this.Description;
    }
}