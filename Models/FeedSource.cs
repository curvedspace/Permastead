
namespace Models;

public class FeedSource : CodeTable
{
    public FeedSource()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
    }

    public FeedSource(long id) : base()
    {
        this.Id = id;
    }
}