using DataAccess;
using Models;

namespace Services;

public class HarvestService
{
    public static List<Harvest> GetAllHarvests(ServiceMode mode)
    {
        var harvests = new List<Harvest>();

        if (mode == ServiceMode.Local)
        {
            //myAnimals = HarvestRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            harvests = DataAccess.Server.HarvestRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return harvests;
    }
}