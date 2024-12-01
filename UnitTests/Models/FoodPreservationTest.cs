namespace UnitTests.Models;

public class FoodPreservationTest
{

    [Fact]
    public void TestCreation()
    {
        var foodPres = new FoodPreservation()
        {
            Id = 1,
            Name = "My Saurkraut"
        };

        Assert.NotNull(foodPres);
        Assert.Equal(1, foodPres.Id);
        Assert.Equal("My Saurkraut", foodPres.Name);

        Assert.NotNull(foodPres.Units);
        Assert.NotNull(foodPres.Type);

    }

}