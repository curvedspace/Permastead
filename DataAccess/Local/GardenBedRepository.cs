using Dapper;

using Models;

using Microsoft.Data.Sqlite;
using System.Data;


namespace DataAccess.Local;

public class GardenBedRepository
{
    public static List<GardenBed> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM GardenBed ORDER BY Description;";

                return db.Query<GardenBed>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<GardenBed>();
        }
    }
}