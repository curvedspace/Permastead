
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Models;

public class ToDo
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime DueDate { get; set; }
        
    public string DisplayDueDate => this.DueDate.ToShortDateString();

    public long DaysUntilDue => new TimeSpan(DueDate.Ticks - DateTime.Now.Ticks).Days;

    public int PercentDone { get; set; }

    public double TimeWindowPercent
    {
        get
        {
            if (DueDate.Date == DateTime.Now.Date)
            {
                return 1;
            }
            
            if (StartDate.Date >= DateTime.Now.Date)
            {
                return 0;
            }
            else
            {
                return 100 * (DateTime.Today-StartDate.Date) / (DueDate.Date-StartDate.Date);
            }
            
        }
    }

    public string TimeWindowPercentText
    {
        get { return TimeWindowPercent.ToString("F0"); }
    }

    public ToDoType ToDoType { get; set; }

    public long ToDoTypeId { get { return this.ToDoType.Id; } }

    public ToDoStatus ToDoStatus { get; set; }

    public long ToDoStatusId { get { return this.ToDoStatus.Id; } }

    public Person Assigner { get; set; }

    public long AssignerId { get { return this.Assigner.Id; } }

    public Person Assignee { get; set; }

    public long AssigneeId { get { return this.Assignee.Id; } }
    
    public DateTime LastUpdatedDate { get; set; }

    public ToDo()
    {
        this.ToDoStatus = new ToDoStatus();
        this.ToDoType = new ToDoType();
        this.Assigner = Person.Gaia();
        this.Assignee = Person.Gaia();

        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Now.Date;
        this.DueDate = DateTime.Now.AddDays(30);
        this.LastUpdatedDate = DateTime.Now;

        this.Description = string.Empty;
    }
    
}