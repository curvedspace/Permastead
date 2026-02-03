using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Common;
using Dapper;

using Microsoft.Data.Sqlite;

using Models;
using Npgsql;


namespace DataAccess.Server
{
    public static class PersonRepository
    {
        public static List<Person> GetAll(string conn, bool onsiteOnly = false)
        {
            var rtnList = new List<Person>();

            try
            {
                using (IDbConnection db = new NpgsqlConnection(conn))
                {
                    string sqlQuery = "SELECT p.Id, p.FirstName, p.LastName, p.CreationDate, p.StartDate, p.EndDate, p.Company, p.Email, p.Phone, p.OnSite, p.Address, p.Comment, p.Tags FROM Person p ORDER BY LastName;";

                    if (onsiteOnly) sqlQuery = "SELECT p.Id, p.FirstName, p.LastName, p.CreationDate, p.StartDate, p.EndDate, p.Company, p.Email, p.Phone, p.OnSite, p.Address, p.Comment, p.Tags FROM Person p WHERE OnSite is true ORDER BY LastName";
                    
                    db.Open();

                    using (IDbCommand command = db.CreateCommand())
                    {
                        command.CommandText = sqlQuery;

                        var dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            var person = new Person();

                            person.Id = Convert.ToInt64(dr[0].ToString());
                            person.FirstName = dr[1].ToString();
                            person.LastName = dr[2].ToString();
                            person.CreationDate = Convert.ToDateTime(dr[3].ToString());
                            person.StartDate = Convert.ToDateTime(dr[4].ToString());
                            person.EndDate = Convert.ToDateTime(dr[5].ToString());
                            person.Company = dr[6].ToString();
                            person.Email = dr[7].ToString();
                            person.Phone = dr[8].ToString();

                            if (string.IsNullOrEmpty(dr[9].ToString()))
                            {
                                person.OnSite = false;
                            }
                            else
                            {
                                person.OnSite = Convert.ToBoolean(dr[9]);
                            }

                            person.Address = dr[10].ToString();
                            person.Comment = dr[11].ToString();

                            var tagText = dr[12].ToString()!.Trim();

                            if (tagText != null && tagText.Length > 0)
                            {
                                person.Tags = tagText.Trim();
                                person.TagList = tagText.Split(' ').ToList();
                            }

                            rtnList.Add(person);
                        }
                    }
                    
                }

                return rtnList;
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
                using (IDbConnection db = new NpgsqlConnection(conn))
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
        
        public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
        {
            var results = new List<SearchResult>();

            var sql = "SELECT i.Id, CONCAT(i.firstname,' ',i.lastname) as Description, " +
                      "i.CreationDate, i.EndDate  " +
                      "FROM Person i " +
                      "WHERE lower(i.firstname) LIKE '%" + searchText.ToLowerInvariant() + "%' OR lower(i.lastname) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                      "ORDER BY i.CreationDate DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[2].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                        result.Entity.Name = "Person";
                        result.SubType = "Name";
                        result.FieldName = "Name";
                        result.SearchText = TextUtils.GetSubstring(dr[1].ToString()!, 0, DataConnection.SearchTextLength, true);

                        results.Add(result);
                    }
                }
            }

            return results;

        }
        
        public static List<TagData> GetAllTags(string connectionString)
        {
            var tags = new List<TagData>();
            var sql = "SELECT p.tags " +
                      "FROM Person p  " + 
                      "WHERE (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) ";

            var stringTags = new List<string>();
            
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    
                    while (dr.Read())
                    {
                        if (dr[0].ToString()! != "")
                        {
                            var currentTags = dr[0].ToString()!.Trim().Split(' ').ToList();
                            foreach (var tagData in currentTags)
                            {
                                if (!stringTags.Contains(tagData))
                                {
                                    stringTags.Add(tagData);
                                }
                            }
                        }
                    }
                }

                foreach (var stringTag in stringTags)
                {
                    var td = new TagData
                    {
                        TagText = stringTag
                    };
                    tags.Add(td);
                }
            }

            return tags;
        }

        public static bool Insert(Person person)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO Person (FirstName, LastName, CreationDate, StartDate, EndDate, Company, Email, Phone, OnSite, Address, Comment, Tags) " +
                                      "VALUES(@FirstName,@LastName,CURRENT_DATE,@StartDate,@EndDate,@Company,@Email,@Phone,@OnSite,@Address,@Comment, @Tags);";

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
                    using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                    {
                        string sqlQuery =
                            "UPDATE Person SET FirstName = @FirstName, StartDate = @StartDate, EndDate = @EndDate, LastName = @LastName, " +
                            "Company = @Company, Email = @Email, Phone = @Phone, OnSite = @OnSite, Address = @Address, Comment = @Comment, Tags = @Tags " +
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
        
        public static bool Delete(Person person)
        {
            try
            {
                if (person != null)
                {
                    using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                    {
                        string sqlQuery = "DELETE FROM Person WHERE Id = @Id;";
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
                      "p.CreationDate, p.StartDate, p.EndDate, p.Company, p.Email, p.Phone, p.OnSite, p.Address, p.Comment, p.Tags " +
                      "FROM Person p  " +
                      "WHERE p.Id = @id ";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new NpgsqlParameter("@id", id));

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
                        
                        if (string.IsNullOrEmpty(dr[9].ToString()))
                        {
                            person.OnSite = false;
                        }
                        else
                        {
                            person.OnSite = Convert.ToBoolean(dr[9]);
                        }
                        
                        person.Address = dr[10].ToString();
                        person.Comment = dr[11].ToString();
                        
                        var tagText = dr[12].ToString()!.Trim();

                        if (tagText != null && tagText.Length > 0)
                        {
                            person.Tags = tagText.Trim();
                            person.TagList = tagText.Split(' ').ToList();
                        }

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

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = connection.CreateCommand())
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
                      "VALUES(:personId, :comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', :commentTypeId, :authorId) ";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue(":personId", personObs.PersonId);
                    command.Parameters.AddWithValue(":comment", personObs.Comment);
                    command.Parameters.AddWithValue(":commentTypeId", personObs.CommentType!.Id);
                    command.Parameters.AddWithValue(":authorId", personObs.Author!.Id);

                    rtnValue = (command.ExecuteNonQuery() == 1);
                }
            }

            return rtnValue;
        }
        
        public static bool UpdatePersonObservation(string connectionString, Observation obs)
        {
            var rtnValue = false;

            var sql = "UPDATE PersonObservation SET Comment = @Comment  " +
                      "WHERE  id = @Id; ";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue(":id", obs.Id);
                    command.Parameters.AddWithValue(":comment", obs.Comment);

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

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = connection.CreateCommand())
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
        
        public static Observation GetObservationById(string connectionString, long id)
        {
            Observation o = null;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id " +
                      "FROM PersonObservation o, CommentType ct, Person p " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.Id = @id " +
                      "AND p.Id = o.AuthorId ";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new NpgsqlParameter("@id", id));
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new Observation();

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
                        o.AsOfDate = o.CreationDate;
                    
                    }
                }
            }
        
            return o;
        }
    }
}
