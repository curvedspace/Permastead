namespace Models;

public class AnEventType : CodeTable
{
    public AnEventType()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
    }

    public AnEventType(long id) : base()
    {
        this.Id = id;
    }
}