using DataAccess;
using DataAccess.Local;
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
    
    public static List<HarvestType> GetAllHarvestTypes(ServiceMode mode)
    {
        var harvests = new List<HarvestType>();

        if (mode == ServiceMode.Local)
        {
            harvests = HarvestTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            harvests = DataAccess.Server.HarvestTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return harvests;
    }
    
    public static bool CommitRecord(ServiceMode mode, Harvest item)
    {
        bool rtnValue = false;
        
        if (item.Id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                //HarvestRepository.Update(item);
            }
            else
            {
                DataAccess.Server.HarvestRepository.Update(item);
            }
            
        }
        else
        {
            // insert new record
            if (mode == ServiceMode.Local)
            {
                //HarvestRepository.Insert(item);
            }
            else
            {
                DataAccess.Server.HarvestRepository.Insert(item);
            }
        }

        return rtnValue;
        
    }
}