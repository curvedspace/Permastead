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
    
    public string BirthdayString => this.Birthday.ToShortDateString();
    
    public DateTime StartDate { get; set; }
    
    public string StartDateString => this.StartDate.ToShortDateString();
    
    public DateTime EndDate { get; set; }
    
    public string EndDateString => this.EndDate.ToShortDateString();
    
    public Person? Author { get; set; }
    
    public bool IsPet { get; set; }
    
    public bool IsNotPet => !this.IsPet;
    
    public string? Comment { get; set; }
    
    /// <summary>
    /// Indicates is the item is currently active, meaning it's start date is earlier than today
    /// and it's end date is in the future.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrent()
    {
        return (DateTime.UtcNow > StartDate) && (DateTime.UtcNow < EndDate);
    }

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