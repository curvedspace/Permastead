using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class InventoryTypeService
{
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
    
    public static bool CommitRecord(ServiceMode mode, InventoryType? type)
    {
        bool rtnValue = false;
        
        if (type != null)
        {
            if (type.Id > 0)
            {
                if (mode == ServiceMode.Local)
                {
                    InventoryTypeRepository.Update(type);
                }
                else
                {
                    DataAccess.Server.InventoryTypeRepository.Update(type);
                }
            }
            else
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    InventoryTypeRepository.Insert(type);
                }
                else
                {
                    DataAccess.Server.InventoryTypeRepository.Insert(type);
                }
            }
        }

        return rtnValue;
    }
}