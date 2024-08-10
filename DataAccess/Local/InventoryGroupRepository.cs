using System.Data;

using Dapper;
using Models;

using Microsoft.Data.Sqlite;

namespace DataAccess.Local
{
    public class InventoryGroupRepository
    {
        public static List<InventoryGroup> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM InventoryGroup ORDER BY Description;";

                    return db.Query<InventoryGroup>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<InventoryGroup>();
            }
        }
        
        public static bool Insert(InventoryGroup iType)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO InventoryGroup (Description, CreationDate, StartDate, EndDate, AuthorId) " +
                                      "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                    return (db.Execute(sqlQuery, iType) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Update(InventoryGroup group)
        {
            try
            {
                
                if (group != null)
                {
                    using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                    {
                        string sqlQuery =
                            "UPDATE InventoryGroup SET Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
                            "WHERE Id = @Id;";

                        return (db.Execute(sqlQuery, group) == 1);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}