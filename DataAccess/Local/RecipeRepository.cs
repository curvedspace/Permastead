
using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;
using Models;

namespace DataAccess.Local
{
    public class RecipeRepository
    {
        public static bool Insert(Recipe recipe)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Recipe (Code, Description, CreationDate, StartDate, EndDate, AuthorId, FeedSourceId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId, @FeedSourceId);";
        
                    return (db.Execute(sqlQuery, recipe) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
