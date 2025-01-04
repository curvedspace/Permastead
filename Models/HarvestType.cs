namespace Models;

public class HarvestType : CodeTable
{
    public Person Author { get; set; }

    public long AuthorId => this.Author.Id;


    public HarvestType()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Gaia();
    }

    public HarvestType(long id) : base()
    {
        this.Id = id;
    }
}