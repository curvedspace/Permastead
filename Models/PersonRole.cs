using Models;

namespace Models;

public class PersonRole : CodeTable
{
    public static PersonRole Customer()
    {
        var pr = new PersonRole();
        pr.Code = "CUST";
        pr.Description = "Customer";

        return pr;
    }
}