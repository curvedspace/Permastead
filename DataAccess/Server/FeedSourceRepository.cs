using Dapper;
using Models;
using Microsoft.Data.Sqlite;
using System.Data;
using Npgsql;


namespace DataAccess.Server
{
    public static class FeedSourceRepository
    {
        public static FeedSource? GetFromCode(string code)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "SELECT * FROM FeedSource WHERE Code = @Code;";

                    return db.QueryFirstOrDefault<FeedSource>(sqlQuery, new { Code = code });
                }
            }
            catch
            {
                return null;
            }            
        }

        public static FeedSource? GetFromId(long id)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "SELECT * FROM FeedSource WHERE Id = @Id;";

                    return db.QueryFirstOrDefault<FeedSource>(sqlQuery, new { Id = id });
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Insert(FeedSource feedSource)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO FeedSource (Code, Description, CreationDate, StartDate, EndDate) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate);";

                    return (db.Execute(sqlQuery, feedSource) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
