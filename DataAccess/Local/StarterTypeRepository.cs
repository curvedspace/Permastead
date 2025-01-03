using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class StarterTypeRepository
{
    public static List<StarterType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM StarterType ORDER BY Description;";

                return db.Query<StarterType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<StarterType>();
        }
    }
    
    public static bool Insert(StarterType st)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO StarterType (Code, Description, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, st) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}