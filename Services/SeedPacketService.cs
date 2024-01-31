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
}