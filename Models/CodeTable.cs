
namespace Models;

/// <summary>
/// Represent a generic code table data item.
/// </summary>
public abstract class CodeTable : IEquatable<CodeTable>
{
    public long Id { get; set; }

    public string? Code { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public bool IsActive => this.IsCurrent();

    public bool IsTransient => (Id == 0);

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        CodeTable? codeTable = obj as CodeTable;
        if (codeTable == null)
            return false;
        else
            return (codeTable.Id == Id) && (codeTable.Code == Code) && (codeTable.Description == Description);

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Code, Description);
    }

    public bool Equals(CodeTable? codeTable)
    {
        return codeTable != null && (codeTable.Id == Id) && (codeTable.Code == Code) && (codeTable.Description == Description);
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

}
