using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;
using Models;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace DataAccess.Server
{
    public class ToDoTypeRepository
    {
        public static List<ToDoType> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM ToDoType ORDER BY Description;";

                    return db.Query<ToDoType>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<ToDoType>();
            }
        }
        
        public static bool Insert(ToDoType tdType)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO ToDoType (Description, CreationDate, StartDate, EndDate) " +
                        "VALUES(@Description, CURRENT_DATE, @StartDate, @EndDate);";

                    return (db.Execute(sqlQuery, tdType) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
