using DataAccess.Local;
using DataAccess;
using Models;

namespace Services;

public class InventoryService
{

    public static List<Inventory> GetAllInventory(ServiceMode mode)
    {
        var inventoryList = new List<Inventory>();

        if (mode == ServiceMode.Local)
        {
            inventoryList = InventoryRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            inventoryList = DataAccess.Server.InventoryRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return inventoryList;
    }
    
    public static bool CommitRecord(ServiceMode mode, Inventory inventory)
    {
        bool rtnValue = false;
        
        if (inventory.Id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                InventoryRepository.Update(inventory);
            }
            else
            {
                DataAccess.Server.InventoryRepository.Update(inventory);
            }
            
        }
        else
        {
            // insert new record
            if (mode == ServiceMode.Local)
            {
                InventoryRepository.Insert(inventory);
            }
            else
            {
                DataAccess.Server.InventoryRepository.Insert(inventory);
            }
        }

        return rtnValue;
        
    }
    
    public static List<InventoryObservation> GetObservationsForInventoryItem(ServiceMode mode, long id)
    {
        var obs = new List<InventoryObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = InventoryRepository.GetAllObservationsForInventoryItem(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            obs = DataAccess.Server.InventoryRepository.GetAllObservationsForInventoryItem(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static List<InventoryObservation> GetAllPersonObservations(ServiceMode mode)
    {
        var obs = new List<InventoryObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = InventoryRepository.GetAllInventoryObservations(DataConnection.GetLocalDataSource());
        }
        else
        {
            obs = DataAccess.Server.InventoryRepository.GetAllInventoryObservations(DataConnection.GetServerConnectionString());
        }

        return obs;
    }

    public static bool AddInventoryObservation(ServiceMode mode, InventoryObservation obs)
    {
        bool rtnValue = false;
        
        if (obs != null)
        {
            if (obs.Inventory.Id > 0)
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    InventoryRepository.InsertInventoryObservation(
                        DataConnection.GetLocalDataSource(), obs);
                }
                else
                {
                    DataAccess.Server.InventoryRepository.InsertInventoryObservation(
                        DataConnection.GetServerConnectionString(), 
                        obs);
                }
            }
        }

        return rtnValue;
    }
}