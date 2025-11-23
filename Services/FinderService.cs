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
                items = DataAccess.Local.ObservationRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText);
            }
            else
            {
                items = DataAccess.Server.ObservationRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText);
            }
            
            //search ToDos
            if (mode == ServiceMode.Local)
            {
                items.AddRange(DataAccess.Local.ToDoRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText));
            }
            else
            {
                items.AddRange(DataAccess.Server.ToDoRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
            
            //now plants
            if (mode == ServiceMode.Local)
            {
                items.AddRange(DataAccess.Local.PlantRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText));
            }
            else
            {
                items.AddRange(DataAccess.Server.PlantRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
            
            //now inventory
            if (mode == ServiceMode.Local)
            {
                items.AddRange(DataAccess.Local.InventoryRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText));
            }
            else
            {
                items.AddRange(DataAccess.Server.InventoryRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
            
            //now prcedures
            if (mode == ServiceMode.Local)
            {
                items.AddRange(DataAccess.Local.ProceduresRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText));
            }
            else
            {
                items.AddRange(DataAccess.Server.ProceduresRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
            
            //now events
            if (mode == ServiceMode.Local)
            {
                items.AddRange(DataAccess.Local.AnEventRepository.GetSearchResults(DataConnection.GetLocalDataSource(),  searchText));
            }
            else
            {
                items.AddRange(DataAccess.Server.AnEventRepository.GetSearchResults(DataConnection.GetServerConnectionString(),  searchText));
            }
        }
        
        
        return items;
    }
}