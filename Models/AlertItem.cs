namespace Models;

public class AlertItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime AsOfDate { get; set; } = DateTime.Now;

    public string Code { get; set; } = "";

    public string Description { get; set; } = "";

    public string Comment { get; set; } = "";

    public AlertItem()
    {
        
    }

    public AlertItem(string code, string description, string comment)
    {
        Code = code;
        Description = description;
        Comment = comment;
    }
    
}