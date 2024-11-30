using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class AnimalTypeRepsoitory
{
    public static List<AnimalType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM AnimalType ORDER BY Description;";

                return db.Query<AnimalType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<AnimalType>();
        }
    }
}