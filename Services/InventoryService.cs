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
    
    public static List<InventoryGroup> GetAllInventoryGroups(ServiceMode mode)
    {
        var myList = new List<InventoryGroup>();

        if (mode == ServiceMode.Local)
        {
            myList = InventoryGroupRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            myList = DataAccess.Server.InventoryGroupRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return myList;
    }
    
    public static List<InventoryType> GetAllInventoryTypes(ServiceMode mode)
    {
        var myList = new List<InventoryType>();

        if (mode == ServiceMode.Local)
        {
            myList = InventoryTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            myList = DataAccess.Server.InventoryTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return myList;
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
    
}