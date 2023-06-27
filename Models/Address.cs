

namespace Models;

public class Address 
{
    public long Id { get; set; }

    public AddressType? Type { get; set; }

    public string Street1 { get; set; } = string.Empty;

    public string Street2 { get; set; } = string.Empty;

    public string Street3 { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

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

    public static Address None()
    {
        var address = new Address();

        address.Type = new AddressType
        {
            Code = "NA",
            Description = "Not Available"
        };

        address.Street1 = string.Empty;
        address.Street2 = string.Empty;
        address.Street3 = string.Empty;
        address.City = "Unknown";
        address.Country = "Unknown";
        address.PostalCode = "XXX XXX";

        return address;
    }
}