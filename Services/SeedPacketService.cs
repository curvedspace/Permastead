using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class SeedPacketService
{
    public static bool CommitRecord(ServiceMode mode, SeedPacket? seedPacket)
    {
        bool rtnValue = false;
        
        if (seedPacket != null)
        {
            if (seedPacket.Id > 0)
            {
                if (mode == ServiceMode.Local)
                {
                    SeedPacketRepository.Update(seedPacket);
                }
                else
                { 
                    DataAccess.Server.SeedPacketRepository.Update(seedPacket);
                }
            }
            else
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    SeedPacketRepository.Insert(seedPacket);
                }
                else
                {
                    DataAccess.Server.SeedPacketRepository.Insert(seedPacket);
                }
            }
        }

        return rtnValue;
        
    }
    
    public static List<SeedPacketObservation> GetObservationsForSeedPacket(ServiceMode mode, long id)
    {
        var obs = new List<SeedPacketObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = SeedPacketRepository.GetAllObservationsForSeedPacket(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            obs = DataAccess.Server.SeedPacketRepository.GetAllObservationsForSeedPacket(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static bool AddObservation(ServiceMode mode, SeedPacketObservation obs)
    {
        bool rtnValue = false;
        
        if (obs != null)
        {
            if (obs.SeedPacket.Id > 0)
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    SeedPacketRepository.InsertSeedPacketObservation(
                        DataConnection.GetLocalDataSource(), obs);
                }
                else
                {
                    DataAccess.Server.SeedPacketRepository.InsertSeedPacketObservation(
                        DataConnection.GetServerConnectionString(), 
                        obs);
                }
            }
        }

        return rtnValue;
    }
}