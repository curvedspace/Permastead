namespace Models;

public class FoodPreservationType : CodeTable
{
    public Person Author { get; set; }

    public long AuthorId => this.Author.Id;


    public FoodPreservationType()
    {
        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.Author = Person.Gaia();
    }

    public FoodPreservationType(long id) : base()
    {
        this.Id = id;
    }
}