using DataAccess;
using Models;

namespace Services;

public class FinderService
{
    public static List<SearchResult> FindRecords(ServiceMode mode,  string searchText)
    {
        var items = new List<SearchResult>();

        
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            // start with observations
            if (mode == ServiceMode.Local)
            {
                items = DataAccess.Local.ObservationRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText);
            }
            else
            {
                items = DataAccess.Server.ObservationRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText);
            }
            
            //search ToDos
            if (mode == ServiceMode.Local)
            {
                //items = DataAccess.Local.ToDoRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText);
            }
            else
            {
                items.AddRange(DataAccess.Server.ToDoRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
        }
        
        
        return items;
    }
}