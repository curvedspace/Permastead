using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class InventoryGroupService
{
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
    
    public static bool CommitRecord(ServiceMode mode, InventoryGroup? group)
    {
        bool rtnValue = false;
        
        if (group != null)
        {
            if (group.Id > 0)
            {
                if (mode == ServiceMode.Local)
                {
                    InventoryGroupRepository.Update(group);
                }
                else
                {
                    DataAccess.Server.InventoryGroupRepository.Update(group);
                }
            }
            else
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    InventoryGroupRepository.Insert(group);
                }
                else
                {
                    DataAccess.Server.InventoryGroupRepository.Insert(group);
                }
            }
        }

        return rtnValue;
    }
    
}