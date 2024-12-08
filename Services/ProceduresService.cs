using DataAccess;
using Models;

namespace Services;

public class ProceduresService
{
    public static List<StandardOperatingProcedure> GetAllProcedures(ServiceMode mode)
    {
        var sops = new List<StandardOperatingProcedure>();

        if (mode == ServiceMode.Local)
        {
            //sops = AnimalRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            sops = DataAccess.Server.ProceduresRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return sops;
    }
    
    public static bool CommitRecord(ServiceMode mode, StandardOperatingProcedure currentItem)
    {
        bool rtnValue = false;
        
        if (currentItem.Id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                //AnEventRepository.Update(currentItem);
            }
            else
            {
                DataAccess.Server.ProceduresRepository.Update(currentItem);
            }
            
        }
        else
        {
            // insert new record
            if (mode == ServiceMode.Local)
            {
                //AnEventRepository.Insert(currentItem);
            }
            else
            {
                DataAccess.Server.ProceduresRepository.Insert(currentItem);
            }
            
        }
        
        return rtnValue;
    }
}