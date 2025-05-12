
namespace UnitTests.Models;

public class ToDoTests
{
    [Fact]
    public void ToDoTestCreation()
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

    [Fact]
    public void ToDoTestWindowPercent()
    {
        var todo = new ToDo();
        
        todo.StartDate = DateTime.Now.AddDays(-10);
        todo.DueDate = DateTime.Now.AddDays(15);
        
        Assert.InRange(todo.TimeWindowPercent,0,100);
    }
}