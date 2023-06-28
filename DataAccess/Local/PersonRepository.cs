using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Dapper;

using Microsoft.Data.Sqlite;

using Models;


namespace DataAccess.Local
{
    public static class PersonRepository
    {
        public static List<Person> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM Person ORDER BY LastName;";

                    return db.Query<Person>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<Person>();
            }
        }
        
        public static bool Insert(Person person)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Person (FirstName, LastName, CreationDate, StartDate, EndDate) " +
                        "VALUES(@FirstName,@LastName,CURRENT_DATE,@StartDate,@EndDate);";

                    return (db.Execute(sqlQuery, person) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
