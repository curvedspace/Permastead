using Dapper;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Models;


namespace DataAccess.Local
{
    public static class GuildRepository
    {
        public static bool Insert(Guild guild)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Guild (Code, Description, CreationDate, StartDate, EndDate, AuthorId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";
        
                    return (db.Execute(sqlQuery, guild) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
