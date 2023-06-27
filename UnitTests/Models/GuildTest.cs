
namespace UnitTests.Models
{
    public class GuildTest
    {
        [Fact]
        public void TestAddPlant()
        {
            var guild = new Guild
            {
                 Id = 1,
                 Code = "ABC",
                 Description = "My Guild"
            };

            Assert.Empty(guild.Plants);

            var plant = new Plant { Id = 1, Code = "PEA", Description = "Stanhope Pea" };

            guild.AddPlant(plant);
            Assert.NotEmpty(guild.Plants);

        }
    }
}
