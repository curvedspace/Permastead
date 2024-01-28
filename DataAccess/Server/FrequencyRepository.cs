using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class FrequencyRepository
{
    public static List<Frequency> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM Frequency ORDER BY Description;";

                return db.Query<Frequency>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<Frequency>();
        }
    }
    
    public static bool Insert(Frequency f)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO Frequency (Code, Description, AuthorId, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Code, @Description, @AuthorId, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, f) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}