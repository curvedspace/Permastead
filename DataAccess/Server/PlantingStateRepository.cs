using System.Data;
using Microsoft.Data.Sqlite;

using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server
{
    public class PlantingStateRepository
    {
        public static List<PlantingState> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM PlantingState ORDER BY Description;";

                    return db.Query<PlantingState>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<PlantingState>();
            }
        }
        
        public static bool Insert(PlantingState pState)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO PlantingState (Code, Description, CreationDate, StartDate, EndDate) " +
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
}