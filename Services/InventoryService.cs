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

        return inventoryList;
    }
    
    public static List<InventoryGroup> GetAllInventoryGroups(ServiceMode mode)
    {
        var myList = new List<InventoryGroup>();

        if (mode == ServiceMode.Local)
        {
            myList = InventoryGroupRepository.GetAll(DataConnection.GetLocalDataSource());
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

        return myList;
    }
    
}