namespace Models;

public class Fermentation : CodeTable
{
    public Recipe Recipe { get; set; }
    
    public long Rating { get; set; }
    
    public Decimal Amount { get; set; }
    
    public Person Author { get; set; }

    public Fermentation()
    {
        this.Recipe = Recipe.NotAvailable();
        this.Author = Person.Gaia();
    }
}