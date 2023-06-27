using Models;

namespace Models;

public class Location : CodeTable
{
    public Person? Author { get; set; }

    public long AuthorId { get { return this.Author?.Id ?? 0; } }

    public Location()
    {
        this.Author = Person.Anonymous();
    }

    public Location(long id) : this()
    {
        this.Id = id;
    }
}