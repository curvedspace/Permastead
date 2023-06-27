namespace Models;

public class AnEvent
{
    public long Id { get; set; }

    public string? Description { get; set; }
    
    public AnEventType AnEventType { get; set; }
    
    public long AnEventTypeId { get { return this.AnEventType.Id; } }
    
    public Person Assigner { get; set; }

    public long AssignerId { get { return this.Assigner.Id; } }

    public Person Assignee { get; set; }

    public long AssigneeId { get { return this.Assignee.Id; } }
    
    public DateTime CreationDate { get; set; }

    public DateTimeOffset StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Frequency Frequency { get; set; }

    public long FrequencyId { get { return this.Frequency.Id; } }
    
    public bool ToDoTrigger { get; set; }
    
    public long WarningDays { get; set; }
    
    public DateTimeOffset? LastTriggerDate { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public DateTime NextDate
    {
        get
        {
            DateTime rtnDate = DateTime.Today;

            if (LastTriggerDate == null)
            {
                switch (Frequency.Code)
                {
                    case "Y":
                        rtnDate = StartDate.Date.AddYears(1);
                        break;
                    case "M":
                        rtnDate = StartDate.Date.AddMonths(1);
                        break;
                    case "W":
                        rtnDate = StartDate.Date.AddDays(7);
                        break;
                    case "D":
                        rtnDate = StartDate.Date.AddDays(1);
                        break;
                }
            }
            else
            {
                switch (Frequency.Code)
                {
                    case "Y":
                        rtnDate = LastTriggerDate.Value.Date.AddYears(1);
                        break;
                    case "M":
                        rtnDate = LastTriggerDate.Value.Date.AddMonths(1);
                        break;
                    case "W":
                        rtnDate = LastTriggerDate.Value.Date.AddDays(7);
                        break;
                    case "D":
                        rtnDate = LastTriggerDate.Value.Date.AddDays(1);
                        break;
                }
            }
            
            // check for values in the past, meaning we missed the date. In this case set NextDate to now
            if (rtnDate < DateTime.Today && LastUpdatedDate < DateTime.Today)
                rtnDate = DateTime.Today;

            return rtnDate;
        }
    }

    public AnEvent()
    {
        this.Frequency = new Frequency();
        this.AnEventType = new AnEventType();
        this.Assigner = Person.Anonymous();
        this.Assignee = Person.Anonymous();

        this.CreationDate = DateTime.Now;
        this.StartDate = DateTime.Now.Date;
        this.EndDate = DateTime.MaxValue;
        this.LastTriggerDate = CreationDate;
        this.LastUpdatedDate = DateTime.Now;

        this.Description = string.Empty;
    }
}