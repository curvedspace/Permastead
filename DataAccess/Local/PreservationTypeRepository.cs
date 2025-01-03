using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class PreservationTypeRepository
{
     
    public static List<FoodPreservationType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM PreservationType ORDER BY Description;";

                return db.Query<FoodPreservationType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<FoodPreservationType>();
        }
    }
    
    public static bool Insert(FoodPreservationType iType)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO PreservationType (Description, CreationDate, StartDate, EndDate, AuthorId) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                return (db.Execute(sqlQuery, iType) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(FoodPreservationType type)
    {
        try
        {
            
            if (type != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE PreservationType SET Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
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
