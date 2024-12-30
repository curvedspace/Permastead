using DataAccess;
using Models;

namespace Services;

public class FoodPreservationService
{
    public static List<FoodPreservation> GetAll(ServiceMode mode)
    {
        var items = new List<FoodPreservation>();

        if (mode == ServiceMode.Local)
        {
            items = DataAccess.Local.PreservationRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            items = DataAccess.Server.PreservationRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return items;
    }
    
    public static List<FoodPreservationType> GetAllPreservationTypes(ServiceMode mode)
    {
        var items = new List<FoodPreservationType>();

        if (mode == ServiceMode.Local)
        {
            items = DataAccess.Local.PreservationTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            items = DataAccess.Server.PreservationTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return items;
    }
    
    public static bool CommitRecord(ServiceMode mode, FoodPreservation item)
    {
        bool rtnValue = false;
        
        if (item.Id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                DataAccess.Local.PreservationRepository.Update(DataConnection.GetLocalDataSource(), item);
            }
            else
            {
                DataAccess.Server.PreservationRepository.Update(DataConnection.GetServerConnectionString(), item);
            }
            
        }
        else
        {
            // insert new record
            if (mode == ServiceMode.Local)
            {
                DataAccess.Local.PreservationRepository.Insert(DataConnection.GetLocalDataSource(),item);
            }
            else
            {
                DataAccess.Server.PreservationRepository.Insert(DataConnection.GetServerConnectionString(),item);
            }
        }

        return rtnValue;
        
    }
    
    public static List<FoodPreservationObservation> GetObservationsForPreservation(ServiceMode mode, long id)
    {
        var obs = new List<FoodPreservationObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = DataAccess.Local.PreservationRepository.GetAllObservationsForPreservation(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            obs = DataAccess.Server.PreservationRepository.GetAllObservationsForPreservation(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static bool AddPreservationObservation(ServiceMode mode, FoodPreservationObservation obs)
    {
        bool rtnValue = false;
        
        if (obs != null)
        {
            if (obs.FoodPreservation.Id > 0)
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    DataAccess.Local.PreservationRepository.InsertPreservationObservation(
                        DataConnection.GetLocalDataSource(), 
                        obs);
                }
                else
                {
                    DataAccess.Server.PreservationRepository.InsertPreservationObservation(
                        DataConnection.GetServerConnectionString(), 
                        obs);
                }
            }
        }

        return rtnValue;
    }
}