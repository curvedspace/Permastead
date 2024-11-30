using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class AnimalTypeRepository
{
    public static List<AnimalType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
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