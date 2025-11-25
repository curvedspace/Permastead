
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
        Assert.Single(person.Roles);
        Assert.True(person.IsCustomer());
        

    }
}