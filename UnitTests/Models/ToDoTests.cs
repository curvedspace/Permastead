
namespace UnitTests.Models;

public class ToDoTests
{
    [Fact]
    public void RoDoTestCreation()
    {
        var todo = new ToDo();
        Assert.True(todo.Id == 0);
            
        todo.Assigner = Person.Anonymous();
        Assert.True(todo.Assigner.LastName == "Anonymous");

        todo.Id = 1;
        todo.Description = "My first TODO";
        
        var assignee = new Person
        {
            FirstName = "First", 
            LastName = "Last"
        };

        todo.Assignee = assignee;
        
        Assert.True("First Last" == todo.Assignee.FullName());

    }
}