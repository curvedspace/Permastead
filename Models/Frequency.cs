namespace Models;

public class Frequency : CodeTable
{
    public Frequency()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
    }

    public Frequency(long id) : base()
    {
        this.Id = id;
    }
}