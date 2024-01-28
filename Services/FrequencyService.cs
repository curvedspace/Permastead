using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class FrequencyService
{
    public static List<Frequency> GetAll(ServiceMode mode)
    {
        var myEvents = new List<Frequency>();

        if (mode == ServiceMode.Local)
        {
            myEvents = FrequencyRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            myEvents = DataAccess.Server.FrequencyRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return myEvents;
    }
}