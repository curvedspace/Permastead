
namespace Models;

public class Vendor : CodeTable
{
    public Address Address { get; set; }

    public int Rating { get; set; }

    public Vendor()
    {
        this.Id = 0;
        this.CreationDate = DateTime.Now;
        this.Address = new Address();
    }

    public Vendor(long id) : this()
    {
        this.Id = id;
    }
}