
namespace Models;

public class Person 
{
    public long Id { get; set; }

    public string? Prefix { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }
    
    public string? Suffix { get; set; }
    
    public string? Company { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public bool OnSite { get; set; }
    
    public string Tags { get; set; } = "";
    
    public List<string> TagList { get; set; } = new List<string>();
    
    public string? Comment { get; set; }

    public IList<PersonRole> Roles {get; set;}

    public IList<ContactInformation>? Contacts { get; set; }

    public string Address { get; set; } = "";
    
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
    
    public void SyncTags()
    {
        this.Tags = string.Empty;
        foreach (var tag in this.TagList)
        {
            this.Tags += tag + " ";
        }
    }

    public Person()
    {
        this.Roles = new List<PersonRole>();
        this.Roles.Add(PersonRole.Customer());

        this.Contacts = new List<ContactInformation>();

        this.MailingAddress = Models.Address.None();

        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Today;
        this.EndDate = DateTime.MaxValue;
        this.OnSite = false;
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
            MailingAddress = Models.Address.None()
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
            OnSite = true,
            MailingAddress = Models.Address.None()
        };

        return gaia;
    }
}