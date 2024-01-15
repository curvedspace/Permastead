namespace Models;

public class ScoreBoard
{
    public decimal TotalScore{ get; set; }
    
    public int Level { get; set; } = 1;
    
    public long LevelMin { get; set; }
    
    public long LevelMax { get; set; }
    
    public decimal Observations { get; set; }
    
    public decimal Actions { get; set; }
    
    public decimal Events { get; set; }

    public decimal Plantings { get; set; }
    
    public decimal SeedPackets { get; set; }

    public decimal LevelProgress
    {
        get
        {
            if (LevelMax - LevelMin != 0)
                return Math.Round((TotalScore - LevelMin) / (LevelMax - LevelMin), 4);

            else
                return 0;
        }
    }

    public override string ToString()
    {
        return "Total Score: " + string.Format("{0:0.0}", TotalScore) + $"    Level: {Level}";
    }
}