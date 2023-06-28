using System.Collections.Generic;

using Models;
using Services;

namespace UnitTests.Services;

public class ObservationServiceTests
{
    [Fact]
    public void TestObservationsWordCount()
    {
        var obsList = new List<Observation>();
        var obs = new Observation()
        {
            Comment = " Hello my    friend."
        };
        obsList.Add(obs);
        
        obs = new Observation()
        {
            Comment = "This is  the second comment. "
        };
        obsList.Add(obs);
        
        // white space should be ignored
        Assert.Equal(8, ObservationsService.GetObservationWordCount(obsList));

    }
}