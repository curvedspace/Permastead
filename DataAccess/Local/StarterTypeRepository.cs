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
}