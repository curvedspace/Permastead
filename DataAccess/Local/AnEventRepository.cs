using System.Data;
using Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;
using Npgsql;

namespace DataAccess.Local;

public class AnEventRepository
{
    public static List<AnEvent> GetAll(string connectionString)
    {
        var myEvents = new List<AnEvent>();
        AnEvent anEvent;

        try
        {

            var sql = "SELECT e.Id, e.Description, e.EventTypeId, et.Description EventTypeDesc, e.AssignerId, " +
                      "p1.FirstName AssignerFirstName, p1.LastName AssignerLastName, " +
                      "e.AssigneeId, p2.FirstName AssigneeFirstName, p2.LastName AssigneeLastName, " +
                      "e.CreationDate, e.StartDate, e.EndDate, e.FrequencyId, f.Code FreqCode, f.Description FreqDesc, " +
                      "e.ToDoTrigger, e.WarningDays, e.LastTriggerDate, e.LastUpdatedDate " +
                      "FROM Event e, EventType et, Person p1, Person p2, Frequency f " +
                      "WHERE e.EventTypeId = et.Id " +
                      "AND e.AssignerId = p1.Id " +
                      "AND e.AssigneeId = p2.Id " +
                      "AND e.FrequencyId = f.Id " + 
                      "ORDER BY e.LastUpdatedDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        anEvent = new AnEvent();

                        anEvent.Id = Convert.ToInt64(dr[0].ToString());
                        anEvent.Description = dr[1].ToString();
                        anEvent.AnEventType.Id = Convert.ToInt64(dr[2].ToString());
                        anEvent.AnEventType.Description = dr[3].ToString();

                        anEvent.Assigner = new Person();
                        anEvent.Assigner.Id = Convert.ToInt64(dr[4].ToString());
                        anEvent.Assigner.FirstName = dr[5].ToString();
                        anEvent.Assigner.LastName = dr[6].ToString();

                        anEvent.Assignee = new Person();
                        anEvent.Assignee.Id = Convert.ToInt64(dr[7].ToString());
                        anEvent.Assignee.FirstName = dr[8].ToString();
                        anEvent.Assignee.LastName = dr[9].ToString();

                        anEvent.CreationDate = Convert.ToDateTime(dr[10].ToString());
                        anEvent.StartDate = Convert.ToDateTime(dr[11].ToString());
                        anEvent.EndDate = Convert.ToDateTime(dr[12].ToString());

                        anEvent.Frequency = new Frequency();
                        anEvent.Frequency.Id = Convert.ToInt64(dr[13].ToString());
                        anEvent.Frequency.Code = dr[14].ToString()!;
                        anEvent.Frequency.Description = dr[15].ToString()!;

                        anEvent.ToDoTrigger = Convert.ToBoolean(Convert.ToInt32(dr[16].ToString()));
                        anEvent.WarningDays = Convert.ToInt64(dr[17].ToString());
                        anEvent.LastTriggerDate = Convert.ToDateTime(dr[18].ToString());
                        anEvent.LastUpdatedDate = Convert.ToDateTime(dr[19].ToString());

                        myEvents.Add(anEvent);
                    }
                }
            }

            return myEvents;

        }
        catch (Exception)
        {
            return myEvents;
        }
    }
    
    public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
    {
        var results = new List<SearchResult>();

        var sql = "SELECT i.Id, i.Description, " +
                  "i.CreationDate, i.EndDate, i.LastUpdatedDate  " +
                  "FROM Event i " +
                  "WHERE lower(i.Description) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY i.CreationDate DESC";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    var result = new SearchResult();
                    result.AsOfDate = Convert.ToDateTime(dr[2].ToString());
                    result.IsCurrent = true;
                    result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                    result.Entity.Name = "Events";
                    result.SubType = "Description";
                    result.FieldName = "Description";
                    result.SearchText = TextUtils.GetSubstring(dr[1].ToString()!, 0, DataConnection.SearchTextLength, true);

                    results.Add(result);
                }
            }
        }

        return results;

    }
    
    public static bool Insert(AnEvent myEvent)
    {
        try
        {
            if (myEvent != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "INSERT INTO Event (Description, StartDate, EndDate, EventTypeId, AssignerId, AssigneeId, " + 
                        "FrequencyId, ToDoTrigger, WarningDays, LastTriggerDate, LastUpdatedDate, CreationDate) " + 
                        "VALUES (@Description, @StartDate, @EndDate, @AnEventTypeId, @AssignerId, @AssigneeId, " +
                        "@FrequencyId, @ToDoTrigger, @WarningDays, @LastTriggerDate, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

                    return (db.Execute(sqlQuery, myEvent) == 1);
                }
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(AnEvent myEvent)
    {
        try
        {
            if (myEvent != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE Event SET Description = @Description, StartDate = @StartDate, EndDate = @EndDate, EventTypeId = @AnEventTypeId, " +
                        "AssignerId = @AssignerId, AssigneeId = @AssigneeId, FrequencyId = @FrequencyId, ToDoTrigger = @ToDoTrigger, " + 
                        "WarningDays = @WarningDays, LastTriggerDate = @LastTriggerDate, LastUpdatedDate = CURRENT_TIMESTAMP " +
                        "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, myEvent) == 1);
                }
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}

