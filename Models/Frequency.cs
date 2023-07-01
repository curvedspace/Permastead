namespace Models;

public class Frequency : CodeTable
{
    public Person Author { get; set; }
    
    public long AuthorId { get { return this.Author == null ? 0 : this.Author.Id; } }
    
    public Frequency()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Gaia();
    }

    public Frequency(long id) : base()
    {
        this.Id = id;
    }
}