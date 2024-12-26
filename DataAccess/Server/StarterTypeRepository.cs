using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class StarterTypeRepository
{
    public static List<StarterType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
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