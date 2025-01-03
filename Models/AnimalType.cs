namespace Models;

public class AnimalType : CodeTable
{
    public Person Author { get; set; }

    public long AuthorId => this.Author.Id;


    public AnimalType()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Gaia();
    }

    public AnimalType(long id) : base()
    {
        this.Id = id;
    }

}