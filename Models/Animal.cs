namespace Models;

public class Animal
{
    public long Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? NickName { get; set; }
    
    public string? Breed { get; set; }
    
    public AnimalType Type { get; set; }
    public long AnimalTypeId => this.Type.Id;

    public DateTime Birthday { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Person? Author { get; set; }
    
    public string? Comment { get; set; }

    public Animal()
    {
        this.Type = new AnimalType();
    }
    
    public Animal(string? name, string? nickName, string? breed)
    {
        Name = name;
        NickName = nickName;
        Breed = breed;
        this.Type = new AnimalType();
    }
}