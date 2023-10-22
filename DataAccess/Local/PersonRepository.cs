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

        public static bool Update(Person person)
        {
            try
            {
                if (person != null)
                {
                    using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                    {
                        string sqlQuery =
                            "UPDATE Person SET FirstName = @FirstName, StartDate = @StartDate, EndDate = @EndDate, LastName = @LastName " +
                            "WHERE Id = @Id;";

                        return (db.Execute(sqlQuery, person) == 1);
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

        public static Person GetPersonFromId(string connectionString, long id)
        {
            Person person = new Person();

            var sql = "SELECT p.Id, p.FirstName, p.LastName, " +
                      "p.CreationDate, p.StartDate, p.EndDate " +
                      "FROM Person p  " +
                      "WHERE p.Id = @id ";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new SqliteParameter("@id", id));

                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        person = new Person();

                        person.Id = Convert.ToInt64(dr[0].ToString());
                        person.FirstName = dr[1].ToString();
                        person.LastName = dr[2].ToString();
                        person.CreationDate = Convert.ToDateTime(dr[3].ToString());
                        person.StartDate = Convert.ToDateTime(dr[4].ToString());
                        person.EndDate = Convert.ToDateTime(dr[5].ToString());

                    }
                }

                return person;
            }
        }
    }
}
