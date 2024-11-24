namespace Models;

public class Animal
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string NickName { get; set; }
    
    public string Breed { get; set; }
    
    public AnimalType Type { get; set; }
    
    public DateTime Birthday { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}