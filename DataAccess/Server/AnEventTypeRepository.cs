using System.Data;
using Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class AnEventTypeRepository
{
    public static List<AnEventType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM EventType ORDER BY Description;";

                return db.Query<AnEventType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<AnEventType>();
        }
    }
    
    public static AnEventType? GetFromCode(string code)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "SELECT * FROM EventType WHERE Code = @Code;";

                return db.QueryFirstOrDefault<AnEventType>(sqlQuery, new { Code = code });
            }
        }
        catch
        {
            return null;
        }            
    }
    
    public static AnEventType? GetFromId(long id)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "SELECT * FROM EventType WHERE Id = @Id;";

                return db.QueryFirstOrDefault<AnEventType>(sqlQuery, new { Id = id });
            }
        }
        catch
        {
            return null;
        }
    }

    public static bool Insert(AnEventType anEventType)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO EventType (Description, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, anEventType) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
}