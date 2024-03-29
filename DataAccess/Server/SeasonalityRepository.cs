using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class SeasonalityRepository
{
    
    public static List<Seasonality> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(conn))
            {
                string sqlQuery = "SELECT * FROM Seasonality ORDER BY Description;";

                return db.Query<Seasonality>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<Seasonality>();
        }
    }
    
    public static bool Insert(Seasonality pState)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO Seasonality (Code, Description, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, pState) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}