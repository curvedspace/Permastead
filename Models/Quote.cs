using System;

namespace Models;

public class Quote
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.MaxValue;

    public override string ToString()
    {
        return this.Description + " (" + this.AuthorName + ")";
    }

    public Quote()
    {
    }
}