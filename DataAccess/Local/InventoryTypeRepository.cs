using System.Data;

using Dapper;
using Models;

using Microsoft.Data.Sqlite;

namespace DataAccess.Local
{
    public class InventoryTypeRepository
    {
        public static List<InventoryType> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM InventoryType ORDER BY Description;";

                    return db.Query<InventoryType>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<InventoryType>();
            }
        }
        
        public static bool Insert(InventoryType iType)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO InventoryType (Description, CreationDate, StartDate, EndDate, AuthorId) " +
                                      "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                    return (db.Execute(sqlQuery, iType) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}