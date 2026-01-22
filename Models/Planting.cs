using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Serilog.Core;

namespace Models;

public class Planting: CodeTable
{
    public Plant Plant { get; set; }
    
    public long PlantId => this.Plant!.Id;
    
    public SeedPacket SeedPacket { get; set; }
    
    public long SeedPacketId => this.SeedPacket!.Id;
    
    public GardenBed Bed { get; set; }
    
    public long BedId => this.Bed!.Id;

    public decimal YieldRating { get; set; } = 0;
    
    public double YieldRatingValue {get ; set;} = 0;
    
    public PlantingState State { get; set; }
    
    public long PlantingStateId => this.State!.Id;
    
    public bool CreateHarvestToDo {get; set;} = false;

    /// <summary>
    /// Show an option to create a harvest to do but only if it is a new planting and we have a days to harvest value.
    /// </summary>
    public bool HarvestToDoOption => (DaysToHarvest > 0 && this.Id == 0);
    
    public int DaysToHarvest => this.SeedPacket.DaysToHarvest;

    public string Comment { get; set; } 

    public Person Author { get; set; }

    public long AuthorId => this.Author!.Id;

    public string StartDateString => this.StartDate.ToShortDateString();

    public string Age
    {
        get
        {
            string calculatedAge = "Unavailable.";
            DateTime measurementDate = DateTime.Now;

            if (this.EndDate < measurementDate)
            {
                // if there is an end date in the past, the plant is dead.
                // use this end date as the basis for measuring its age.
                measurementDate = this.EndDate;
            }
            
            if (measurementDate < this.StartDate)
            {
                calculatedAge = "Not yet planted.";
            }
            else
            {
                
                if ((measurementDate - this.StartDate).Days <= 365)
                {
                    calculatedAge = (measurementDate - this.StartDate).Days + " day(s).";
                }
                else
                {
                    calculatedAge = ((measurementDate - this.StartDate).Days / 365).ToString("0.##") + " year(s).";
                }
            }

            return calculatedAge;
        }
    }

    public string Colour
    {
        get
        {
            switch (this.State.Code)
            {
                case "DEAD":
                {
                    return "Red";
                    break;
                }
                case "H":
                {
                    return "Gray";
                    break;
                }
                default:
                {
                    return "Green";
                    break;
                }
            }
        }
    }
    
    public Planting()
    {
        this.Code = string.Empty;
        this.Description = string.Empty;
        this.Plant = new Plant();
        this.SeedPacket = new SeedPacket();
        this.State = new PlantingState();
        this.Author = Person.Anonymous();
        this.Bed = new GardenBed();
        this.Comment = string.Empty;
    }

    public Planting Clone(Planting planting)
    {
        var copy = new Planting();

        if (planting != null)
        {
            copy.Comment = planting.Comment;
            copy.State = planting.State;
            copy.SeedPacket = planting.SeedPacket.Clone(planting.SeedPacket);
            copy.Author = planting.Author;
            copy.YieldRating = planting.YieldRating;
            copy.Bed = planting.Bed;
            copy.Plant =  planting.Plant;
            copy.StartDate = DateTime.UtcNow;
            copy.Code  = planting.Code;
            copy.Description = planting.Description;
        }
        
        return copy;
    }

}