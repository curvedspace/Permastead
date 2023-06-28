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

        return myEvents;
    }
        
    public static List<AnEventType> GetAllEventTypes(ServiceMode mode)
    {
        var eventTypes = new List<AnEventType>();

        if (mode == ServiceMode.Local)
        {
            eventTypes = AnEventTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }

        return eventTypes;
    }
}