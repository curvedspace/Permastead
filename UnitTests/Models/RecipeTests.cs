
namespace UnitTests.Models;

public class RecipeTests
{
    [Fact]
    public void RecipeTestCreation()
    {
        var r = new Recipe();
        Assert.True(r.IsCurrent());

        r.Author = Person.Anonymous();
        Assert.True(r.Author.LastName == "Anonymous");

        r.Id = 1;
        r.Code = "BASICMEAD";
        r.Description = "My Basic Mead Recipe";

        var i1 = new Ingredient("HONEY", "Honey");
        var i2 = new Ingredient("WATER", "Water");

        r.Ingredients.Add(i1);
        r.Ingredients.Add(i2);

        Assert.True(r.NumberOfIngredients == 2);

    }

    [Fact]
    public void RecipeTestSteps()
    {
        var r = new Recipe();
        Assert.True(r.IsCurrent());

        r.Author = Person.Anonymous();
        Assert.True(r.Author.LastName == "Anonymous");

        r.Id = 1;
        r.Code = "BASICMEAD";
        r.Description = "My Basic Mead Recipe";

        var s1 = "Add the water to the pot";
        var s2 = "Add the honey";
        var s3 = "Add the yeast";

        r.Steps.Add(s1);
        r.Steps.Add(s2);
        r.Steps.Add(s3);

        Assert.True(r.NumberOfSteps == 3);

    }
}
