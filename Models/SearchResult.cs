namespace Models;

public class SearchResult
{
    public DateTime AsOfDate { get; set; }
    
    public string AsOfDateDateString => this.AsOfDate.ToShortDateString();
    
    public Entity Entity { get; set; }
    
    public string SubType { get; set; }
    
    public string FieldName { get; set; }
    
    public string SearchText { get; set; }
    
    public bool IsCurrent { get; set; }

    public SearchResult()
    {
        AsOfDate = DateTime.Now;
        Entity = new Entity();
        SubType = "";
        FieldName = "";
        SearchText = "";
        IsCurrent = false;
    }
}