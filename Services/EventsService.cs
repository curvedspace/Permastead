using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

public class EventsService
{
    public static List<AnEvent> GetAllEvents(ServiceMode mode)
    {
        var myEvents = new List<AnEvent>();

        if (mode == ServiceMode.Local)
        {
            myEvents = AnEventRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            myEvents = DataAccess.Server.AnEventRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return myEvents;
    }
        
    public static List<AnEventType> GetAllEventTypes(ServiceMode mode)
    {
        var eventTypes = new List<AnEventType>();

        if (mode == ServiceMode.Local)
        {
            eventTypes = AnEventTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            eventTypes = DataAccess.Server.AnEventTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return eventTypes;
    }

    public static bool CommitRecord(ServiceMode mode, AnEvent currentItem)
    {
        bool rtnValue = false;
        
        if (currentItem.Id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                AnEventRepository.Update(currentItem);
            }
            else
            {
                DataAccess.Server.AnEventRepository.Update(currentItem);
            }
            
        }
        else
        {
            // insert new record
            if (mode == ServiceMode.Local)
            {
                AnEventRepository.Insert(currentItem);
            }
            else
            {
                DataAccess.Server.AnEventRepository.Insert(currentItem);
            }
            
        }
        
        return rtnValue;
    }
}