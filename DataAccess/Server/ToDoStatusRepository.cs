using Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;
using Npgsql;

namespace DataAccess.Server
{
    public class ToDoStatusRepository
    {
        public static List<ToDoStatus> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM ToDoStatus ORDER BY Description;";

                    return db.Query<ToDoStatus>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<ToDoStatus>();
            }
        }
        
        public static bool Insert(ToDoStatus tdStatus)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO ToDoStatus (Description, CreationDate, StartDate, EndDate) " +
                        "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate);";

                    return (db.Execute(sqlQuery, tdStatus) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
