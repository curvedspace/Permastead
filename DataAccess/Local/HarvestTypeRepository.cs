using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class HarvestTypeRepository
{
    
    public static List<HarvestType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM HarvestType ORDER BY Description;";

                return db.Query<HarvestType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<HarvestType>();
        }
    }
    
    public static bool Insert(HarvestType iType)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO HarvestType (Description, CreationDate, StartDate, EndDate, AuthorId) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                return (db.Execute(sqlQuery, iType) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(HarvestType type)
    {
        try
        {
            
            if (type != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE HarvestType SET Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
                        "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, type) == 1);
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
