using static System.String;

namespace Models;

public class ToDoStatus 
{
    public long Id { get; set; }

    public string Description { get; set; } = Empty;

    public bool IsActive { get; set; }

    public bool IsTransient { get
    {
        if ((Id == 0)) return true;
        return false;
    } }
    
    public bool Equals(CodeTable codeTable)
    {
        return (codeTable.Id == Id) && (codeTable.Description == Description);
    }

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

    public ToDoStatus()
    {
        this.StartDate = DateTime.Now;
        this.CreationDate = DateTime.Now;
        this.EndDate = DateTime.MaxValue;
    }

    public ToDoStatus(long id) : this()
    {
        this.Id = id;
    }
    
    public override string ToString()
    {
        return this.Description;
    }
}