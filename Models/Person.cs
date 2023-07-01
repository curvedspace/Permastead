
namespace Models;

public class Person 
{
    public long Id { get; set; }

    public string? Prefix { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Suffix { get; set; }

    public IList<PersonRole> Roles {get; set;}

    public IList<ContactInformation>? Contacts { get; set; }

    public Address? MailingAddress { get; set; }

    public string FullName()
    {
        if (string.IsNullOrEmpty(MiddleName))
        {
            return (this.Prefix + " " + this.FirstName + " " + this.LastName).Trim();
        }
        else
        {
            return (this.Prefix + " " + this.FirstName + " " + this.MiddleName + " " + this.LastName).Trim();
        }
    }

    public string FullNameLastFirst => (this.LastName + ", " + this.Prefix + " " + this.FirstName + " " + this.MiddleName).Trim();

    public bool IsCustomer()
    {
        foreach (PersonRole pr in this.Roles)
        {
            if (pr.Code == "CUST") return true;
        }

        return false;
    }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.MaxValue;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates is the item is currently active, meaning it's start date is earlier than today
    /// and it's end date is in the future.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrent()
    {
        if ((DateTime.UtcNow > StartDate) && (DateTime.UtcNow < EndDate))
            return true;
        else
            return false;
    }

    public bool IsInThePast()
    {
        return DateTime.UtcNow > StartDate;
    }

    public bool IsInTheFuture()
    {
        return !IsInThePast();
    }

    public Person()
    {
        this.Roles = new List<PersonRole>();
        this.Roles.Add(PersonRole.Customer());

        this.Contacts = new List<ContactInformation>();

        this.MailingAddress = Address.None();

        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
    }

    public Person(long id) : this()
    {
        this.Id = id;
    }

    public static Person Anonymous()
    {
        var anon = new Person
        {
            FirstName = "Anonymous",
            LastName = "Anonymous",
            MailingAddress = Address.None()
        };

        return anon;
    }
    
    public static Person Gaia()
    {
        var gaia = new Person
        {
            Id = 1,
            FirstName = "Gaia",
            LastName = "AI",
            MailingAddress = Address.None()
        };

        return gaia;
    }
}