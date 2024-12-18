using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class HarvestTypeRepository
{
    public static List<HarvestType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM InventoryType ORDER BY Description;";

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
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
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
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
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
