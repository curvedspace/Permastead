using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class MeasurementUnitRepository
{
    public static List<MeasurementUnit> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM MeasurementType ORDER BY Description;";

                return db.Query<MeasurementUnit>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<MeasurementUnit>();
        }
    }
        
    public static bool Insert(MeasurementUnit iType)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO MeasurementType (Description, CreationDate, StartDate, EndDate, AuthorId) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                return (db.Execute(sqlQuery, iType) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(MeasurementUnit type)
    {
        try
        {
                
            if (type != null)
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery =
                        "UPDATE MeasurementType SET Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
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
