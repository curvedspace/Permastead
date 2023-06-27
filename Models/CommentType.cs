using Models;

namespace Models;

public class CommentType : CodeTable
{
    public Person? Author { get; set; }

    public long AuthorId { get { return this.Author?.Id ?? 0; } }

    public CommentType()
    {
        this.Author = Person.Anonymous();
    }

    public CommentType(long id) : this()
    {
        this.Id = id;
    }
}