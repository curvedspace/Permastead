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
    
    public long AuthorId => this.Author!.Id;
    
    public bool IsPet { get; set; }
    
    public bool IsNotPet => !this.IsPet;
    
    public string Tags { get; set; } = "";
    
    public List<string> TagList { get; set; } = new List<string>();
    
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
    
    public string Age
    {
        get
        {
            string calculatedAge = "Unavailable.";
            DateTime measurementDate = DateTime.Now;

            if (this.EndDate < measurementDate)
            {
                // if there is an end date in the past, the plant is dead.
                // use this end date as the basis for measuring its age.
                measurementDate = this.EndDate;
            }
            
            if (measurementDate < this.Birthday)
            {
                calculatedAge = "Not yet planted.";
            }
            else
            {
                
                if ((measurementDate - this.Birthday).Days <= 365)
                {
                    calculatedAge = (measurementDate - this.Birthday).Days + " day(s).";
                }
                else
                {
                    calculatedAge = ((measurementDate - this.Birthday).Days / 365).ToString("0.##") + " year(s).";
                }
            }

            return calculatedAge;
        }
    }
    
    public void SyncTags()
    {
        this.Tags = string.Empty;
        foreach (var tag in this.TagList)
        {
            this.Tags += tag + " ";
        }
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
        Author = Person.Gaia();
        
        this.Type = new AnimalType();
    }
}