
namespace UnitTests.Models;

public class PersonTest
{
    [Fact]
    public void TestPersonFullname()
    {
        var person = new Person
        {
            FirstName = "Jane",
            LastName = "Homesteader"
        };

        Assert.Equal("Jane Homesteader", person.FullName());
        Assert.Equal(1, person.Roles.Count);
        Assert.True(person.IsCustomer());
        

    }
}