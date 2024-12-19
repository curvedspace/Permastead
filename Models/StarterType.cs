namespace Models;

public class StarterType : CodeTable
{
    public Person Author { get; set; }

    public long AuthorId => this.Author.Id;


    public StarterType()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Anonymous();
    }

    public StarterType(long id) : base()
    {
        this.Id = id;
    }
    
}