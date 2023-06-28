using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class FrequencyRepository
{
    public static List<Frequency> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
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
}