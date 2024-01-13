

namespace Models;

public class Plant : CodeTable
{
    public string PlantType { get; set; } = "Unknown";

    public string Comment { get; set; } = "";

    public string Family { get; set; } = "Unknown";

    public string Url { get; set; } = string.Empty;

    public Person? Author { get; set; }

    public long AuthorId => this.Author!.Id;

    public Plant()
    {
        this.Code = string.Empty;
        this.Description = string.Empty;
        this.Author = Person.Gaia();
    }
    
    public Plant(long id) : this()
    {
        this.Id = id;
    }

}