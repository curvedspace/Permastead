using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class ProceduresService
{
    public static List<StandardOperatingProcedure> GetAllProcedures(ServiceMode mode)
    {
        var sops = new List<StandardOperatingProcedure>();

        if (mode == ServiceMode.Local)
        {
            sops = ProceduresRepository.GetAll(DataConnection.GetLocalDataSource());
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
                ProceduresRepository.Update(currentItem);
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
                ProceduresRepository.Insert(currentItem);
            }
            else
            {
                DataAccess.Server.ProceduresRepository.Insert(currentItem);
            }
        }
        
        return rtnValue;
    }
}