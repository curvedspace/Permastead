namespace UnitTests.Models;

public class HarvestTest
{
    [Fact]
    public void TestCreation()
    {
        var harvest = new Harvest()
        {
            Id = 1,
            Description = "My Harvest"
        };

        Assert.NotNull(harvest);
        Assert.Equal(1, harvest.Id);
        Assert.Equal("My Harvest", harvest.Description);

        Assert.NotNull(harvest.Units);
        Assert.NotNull(harvest.Type);

    }
}
