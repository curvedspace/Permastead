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
        public static List<Person> GetAll(string conn, bool onsiteOnly = false)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM Person ORDER BY LastName";
                    
                    if (onsiteOnly) sqlQuery = "SELECT * FROM Person WHERE OnSite = 1 ORDER BY LastName";

                    return db.Query<Person>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<Person>();
            }
        }

        public static List<string> GetAllCompanies(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT DISTINCT p.Company FROM Person p WHERE p.Company != '' ORDER BY Company;";

                    return db.Query<string>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<string>();
            }
        }

        public static bool Insert(Person person)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Person (FirstName, LastName, CreationDate, StartDate, EndDate, Company, Email, Phone, Comment) " +
                                      "VALUES(@FirstName,@LastName,CURRENT_DATE,@StartDate,@EndDate,@Company,@Email,@Phone,@Comment);";

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
                            "UPDATE Person SET FirstName = @FirstName, StartDate = @StartDate, EndDate = @EndDate, LastName = @LastName, " +
                            "Company = @Company, Email = @Email, Phone = @Phone, Comment = @Comment " +
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
                      "p.CreationDate, p.StartDate, p.EndDate, p.Company, p.Email, p.Phone, p.Comment " +
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
                        person.Company = dr[6].ToString();
                        person.Email = dr[7].ToString();
                        person.Phone = dr[8].ToString();
                        person.Comment = dr[9].ToString();
                    }
                }

                return person;
            }
        }
        
        public static List<PersonObservation> GetAllObservationsForPerson(string connectionString, long personId)
        {
            {
                var myObs = new List<PersonObservation>();
                PersonObservation o;

                var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                          "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.PersonId, pp.FirstName, pp.LastName " +
                          "FROM PersonObservation o, CommentType ct, Person p, Person pp " +
                          "WHERE ct.Id = o.CommentTypeId " +
                          "AND o.PersonId = pp.Id " +
                          "AND o.PersonId = @Id " +
                          "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@Id", personId);
                        
                        var dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            o = new PersonObservation();
                            
                            o.Comment = dr[0].ToString()!;
                            o.CreationDate = Convert.ToDateTime(dr[1].ToString());
                            o.StartDate = Convert.ToDateTime(dr[2].ToString());
                            o.EndDate = Convert.ToDateTime(dr[3].ToString());

                            o.CommentType = new CommentType();
                            o.CommentType.Id = Convert.ToInt64(dr[4].ToString());
                            o.CommentType.Description = dr[5].ToString();

                            o.Author = new Person();
                            o.Author.Id = Convert.ToInt64(dr[6].ToString());
                            o.Author.FirstName = dr[7].ToString();
                            o.Author.LastName = dr[8].ToString();

                            o.Id = Convert.ToInt64(dr[9].ToString());
                            o.Person.Id = Convert.ToInt64(dr[10].ToString());
                            o.Person.FirstName = dr[11].ToString();
                            o.Person.LastName = dr[12].ToString();
                            
                            o.AsOfDate = o.CreationDate;

                            myObs.Add(o);
                        }
                    }

                    return myObs;
                }
            }
        }
        
        public static bool InsertPersonObservation(string connectionString, PersonObservation personObs)
        {
            var rtnValue = false;

            var sql = "INSERT INTO PersonObservation (PersonId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                      "VALUES($personId, $comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', $commentTypeId, $authorId) ";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("personId", personObs.PersonId);
                    command.Parameters.AddWithValue("$comment", personObs.Comment);
                    command.Parameters.AddWithValue("$commentTypeId", personObs.CommentType!.Id);
                    command.Parameters.AddWithValue("$authorId", personObs.Author!.Id);

                    rtnValue = (command.ExecuteNonQuery() == 1);
                }
            }

            return rtnValue;
        }
        
        public static List<PersonObservation> GetAllPersonObservations(string connectionString)
        {
            {
                var myObs = new List<PersonObservation>();
                PersonObservation o;

                var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                          "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.PersonId, pp.FirstName, pp.LastName " +
                          "FROM PersonObservation o, CommentType ct, Person p, Person pp " +
                          "WHERE ct.Id = o.CommentTypeId " +
                          "AND o.PersonId = pp.Id " +
                          "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        
                        var dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            o = new PersonObservation();
                            
                            o.Comment = dr[0].ToString()!;
                            o.CreationDate = Convert.ToDateTime(dr[1].ToString());
                            o.StartDate = Convert.ToDateTime(dr[2].ToString());
                            o.EndDate = Convert.ToDateTime(dr[3].ToString());

                            o.CommentType = new CommentType();
                            o.CommentType.Id = Convert.ToInt64(dr[4].ToString());
                            o.CommentType.Description = dr[5].ToString();

                            o.Author = new Person();
                            o.Author.Id = Convert.ToInt64(dr[6].ToString());
                            o.Author.FirstName = dr[7].ToString();
                            o.Author.LastName = dr[8].ToString();

                            o.Id = Convert.ToInt64(dr[9].ToString());
                            o.Person.Id = Convert.ToInt64(dr[10].ToString());
                            o.Person.FirstName = dr[11].ToString();
                            o.Person.LastName = dr[12].ToString();
                            
                            o.AsOfDate = o.CreationDate;

                            myObs.Add(o);
                        }
                    }

                    return myObs;
                }
            }
        }
    }
}
