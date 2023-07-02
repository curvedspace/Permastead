using System.Data;
using Microsoft.Data.Sqlite;

using Dapper;
using Models;

namespace DataAccess.Local
{
    public class PlantingStateRepository
    {
        public static List<PlantingState> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
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
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
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