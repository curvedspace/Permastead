namespace Models;

public class Achievement
{
    public AchievementType Type { get; set; }

    public decimal InitialPoints { get; set; } = 100;

    public decimal IterationMultiplier { get; set; } = 0.5m;
    
    public long Count { get; set; }

    public decimal CurrentPoints
    {
        get
        {
            if (this.Count == 0)
            {
                return InitialPoints;
            }
            else if (this.Count == 1)
            {
                return InitialPoints;
            }
            else
            {
                // we are looking for to drop points offered as the number of records goes up
                var multiplier = (decimal)Math.Pow((double)IterationMultiplier, Count-1);
                var computedPoints = InitialPoints * multiplier;
                if (computedPoints < 0.5m) computedPoints = 0.5m;     //if current points awarded is less than 0.1, cap it there

                return computedPoints;
            }
        }
    }
}
