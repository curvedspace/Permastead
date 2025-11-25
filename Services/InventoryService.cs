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
    
    public static Inventory GetInventoryFromId(ServiceMode mode, long id)
    {
        var inventory = new Inventory();

        if (mode == ServiceMode.Local)
        {
            inventory = InventoryRepository.GetInventoryFromId(DataConnection.GetLocalDataSource(),id);
        }
        else
        {
            inventory = DataAccess.Server.InventoryRepository.GetInventoryFromId(DataConnection.GetServerConnectionString(),id);
        }

        return inventory;
    }
    
    public static List<string> GetAllBrands(ServiceMode mode)
    {
        var brands = new List<string>();

        if (mode == ServiceMode.Local)
        {
            brands = InventoryRepository.GetAllBrands(DataConnection.GetLocalDataSource());
        }
        else
        {
            brands = DataAccess.Server.InventoryRepository.GetAllBrands(DataConnection.GetServerConnectionString());
        }

        return brands;
    }
    
    public static List<string> GetAllRooms(ServiceMode mode)
    {
        var rooms = new List<string>();

        if (mode == ServiceMode.Local)
        {
            rooms = InventoryRepository.GetAllRooms(DataConnection.GetLocalDataSource());
        }
        else
        {
            rooms = DataAccess.Server.InventoryRepository.GetAllRooms(DataConnection.GetServerConnectionString());
        }

        return rooms;
    }
    
    public static List<string> GetAllGroups(ServiceMode mode)
    {
        var list = new List<string>();

        if (mode == ServiceMode.Local)
        {
            //get the list from the table
            list = InventoryRepository.GetAllGroups(DataConnection.GetLocalDataSource());
            
            //supplement with the reference table
            var defaults = InventoryGroupService.GetAllInventoryGroups(mode);
            foreach (var group in defaults)
            {
                if (!list.Contains(group.Description))
                {
                    list.Add(group.Description);
                }
            }

            list.Sort();
            
        }
        else
        {
            list = DataAccess.Server.InventoryRepository.GetAllGroups(DataConnection.GetServerConnectionString());
            
            //supplement with the reference table
            var defaults = InventoryGroupService.GetAllInventoryGroups(mode);
            foreach (var group in defaults)
            {
                if (!list.Contains(group.Description))
                {
                    list.Add(group.Description);
                }
            }

            list.Sort();
            
        }

        return list;
    }
    
    public static List<string> GetAllTypes(ServiceMode mode)
    {
        var list = new List<string>();

        if (mode == ServiceMode.Local)
        {
            list = InventoryRepository.GetAllBrands(DataConnection.GetLocalDataSource());
            
            //supplement with the reference table
            var defaults = InventoryTypeService.GetAllInventoryTypes(mode);
            foreach (var inventoryType in defaults)
            {
                if (!list.Contains(inventoryType.Description))
                {
                    list.Add(inventoryType.Description);
                }
            }

            list.Sort();
        }
        else
        {
            list = DataAccess.Server.InventoryRepository.GetAllTypes(DataConnection.GetServerConnectionString());
            
            //supplement with the reference table
            var defaults = InventoryTypeService.GetAllInventoryTypes(mode);
            foreach (var inventoryType in defaults)
            {
                if (!list.Contains(inventoryType.Description))
                {
                    list.Add(inventoryType.Description);
                }
            }

            list.Sort();
        }

        return list;
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