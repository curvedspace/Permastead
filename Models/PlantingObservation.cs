namespace Models;

public class PlantingObservation
{
    #region Properties
    
    public long Id { get; set; }

    public Planting Planting { get; set; }
    
    public long PlantingId { get { return this.Planting?.Id ?? 0; } }
    
    public DateTime AsOfDate { get; set; }

    public string DisplayAsOfDate => this.AsOfDate.ToShortDateString();

    public CommentType? CommentType { get; set; }

    public long CommentTypeId { get { return this.CommentType?.Id ?? 0; } }

    public string Comment { get; set; } = string.Empty;

    public Person? Author { get; set; }

    public long AuthorId { get { return this.Author?.Id ?? 0; } }

    public string FullDescription => DisplayAsOfDate + ": " + Comment;

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

    #endregion

    #region Constructors

    public PlantingObservation()
    {
        this.AsOfDate = DateTime.UtcNow;
        this.Author = Person.Anonymous();
        this.CommentType = new CommentType();
        this.Planting = new Planting();
    }
    
    public PlantingObservation(string comment) : this()
    {
        this.Comment = comment;
    }

 
    #endregion
}