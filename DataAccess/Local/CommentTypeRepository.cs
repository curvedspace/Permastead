using Models;
using Microsoft.Data.Sqlite;
using System.Data;

using Dapper;

namespace DataAccess.Local
{
    public class CommentTypeRepository
    {
        public static List<CommentType> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM CommentType;";

                    return db.Query<CommentType>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<CommentType>();
            }
        }
        
        public static bool Insert(CommentType commentType)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO CommentType (Code, Description, CreationDate, StartDate, EndDate, AuthorId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                    return (db.Execute(sqlQuery, commentType) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
