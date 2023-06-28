using Microsoft.Data.Sqlite;

using System;
using System.Data;
using System.Collections.Generic;

using Dapper;
using Models;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace DataAccess.Local
{
    public static class ObservationRepository
    {
        public static List<Observation> GetObservations(string connectionString)
        {
            var myObs = new List<Observation>();
            Observation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id " +
                "FROM Observation o, CommentType ct, Person p " +
                "WHERE ct.Id = o.CommentTypeId " +
                "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
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

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }

        public static StringBuilder SearchObservations(string connectionString, string searchTerm)
        {
            var sql = "SELECT o.Comment, o.CreationDate FROM Observation o WHERE o.Comment LIKE '%" + searchTerm + "%' ORDER BY o.CreationDate DESC";
            var sb = new StringBuilder();

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        sb.AppendLine("(" + Convert.ToDateTime(dr[1].ToString()).ToString("yyyy-MMM-dd") + "): " + dr[0].ToString());

                    }
                }
            }

            return sb;
        }

        public static bool InsertObservation(string connectionString, Observation obs)
        {
            var rtnValue = false;

            var sql = "INSERT INTO Observation (Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                "VALUES($comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', $commentTypeId, $authorId) ";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("$comment", obs.Comment);
                    command.Parameters.AddWithValue("$commentTypeId", obs.CommentType!.Id);
                    command.Parameters.AddWithValue("$authorId", obs.Author!.Id);

                    rtnValue = (command.ExecuteNonQuery() == 1);
                }
            }

            return rtnValue;
        }

        public static bool Insert(Observation observation)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Observation (Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                        "VALUES(@Comment, CURRENT_DATE, @StartDate, @EndDate, @CommentTypeId, @AuthorId);";

                    return (db.Execute(sqlQuery, observation) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
