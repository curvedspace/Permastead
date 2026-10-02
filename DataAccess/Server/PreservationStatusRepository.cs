using Models;

using System.Data;

using Dapper;
using Npgsql;


namespace DataAccess.Server;

public class PreservationStatusRepository
{
    public static List<PreservationStatus> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM PreservationStatus ORDER BY Description;";

                return db.Query<PreservationStatus>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<PreservationStatus>();
        }
    }
        
    public static bool Insert(PreservationStatus preservationStatus)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO PreservationStatus (Description, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, preservationStatus) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}
