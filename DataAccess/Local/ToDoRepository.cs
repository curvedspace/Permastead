using Dapper;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using Common;
using Models;

namespace DataAccess.Local
{
    public class ToDoRepository
    {
        public static List<ToDo> GetAllToDos(string connectionString)
        {
            var myTodos = new List<ToDo>();
            ToDo todo;

            var sql = "SELECT td.Id, td.Description, td.CreationDate, td.StartDate, td.DueDate, td.PercentDone, " +
                "tdt.Id, tdt.Description, tds.Id, tds.Description,  " +
                "p1.Id, p1.FirstName, p1.LastName, " +
                "p2.Id Id2, p2.FirstName fn, p2.LastName ln, td.LastUpdatedDate " +
                "FROM ToDo td, ToDoType tdt, ToDoStatus tds, Person p1, Person p2 " +
                "WHERE td.ToDoTypeId = tdt.Id " +
                "AND td.ToDoStatusId = tds.Id " +
                "AND td.AssignerId = p1.Id " +
                "AND td.AssigneeId = p2.Id ORDER BY td.DueDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        todo = new ToDo();

                        todo.Id = Convert.ToInt64(dr[0].ToString());
                        todo.Description = dr[1].ToString();
                        todo.CreationDate = Convert.ToDateTime(dr[2].ToString());
                        todo.StartDate = Convert.ToDateTime(dr[3].ToString());
                        todo.DueDate = DateTime.Parse(dr[4].ToString());
                        todo.PercentDone = Convert.ToInt16(dr[5].ToString());

                        todo.ToDoType = new ToDoType();
                        todo.ToDoType.Id = Convert.ToInt64(dr[6].ToString());
                        todo.ToDoType.Description = dr[7].ToString()!;

                        todo.ToDoStatus = new ToDoStatus();
                        todo.ToDoStatus.Id = Convert.ToInt64(dr[8].ToString());
                        todo.ToDoStatus.Description = dr[9].ToString()!;

                        todo.Assigner = new Person();
                        todo.Assigner.Id = Convert.ToInt64(dr[10].ToString());
                        todo.Assigner.FirstName = dr[11].ToString();
                        todo.Assigner.LastName = dr[12].ToString();

                        todo.Assignee = new Person();
                        todo.Assignee.Id = Convert.ToInt64(dr[13].ToString());
                        todo.Assignee.FirstName = dr[14].ToString();
                        todo.Assignee.LastName = dr[15].ToString();
                        
                        todo.LastUpdatedDate = Convert.ToDateTime(dr[16].ToString());

                        myTodos.Add(todo);
                    }
                }

                return myTodos;
            }
        }

        public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
        {
            var results = new List<SearchResult>();

            var sql = "SELECT td.Id, td.Description, td.CreationDate, td.StartDate, td.DueDate, td.PercentDone, " +
                "tdt.Id, tdt.Description, tds.Id, tds.Description,  " +
                "p1.Id, p1.FirstName, p1.LastName, " +
                "p2.Id Id2, p2.FirstName fn, p2.LastName ln, td.LastUpdatedDate " +
                "FROM ToDo td, ToDoType tdt, ToDoStatus tds, Person p1, Person p2 " +
                "WHERE td.ToDoTypeId = tdt.Id " +
                "AND td.ToDoStatusId = tds.Id " +
                "AND td.AssignerId = p1.Id " +
                "AND lower(td.Description) LIKE '%" + searchText.ToLowerInvariant() + "%'" +
                "AND td.AssigneeId = p2.Id ORDER BY td.DueDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
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
                        result.Entity.Id =  Convert.ToInt64(dr[0].ToString());
                        result.Entity.Name = "Action";
                        result.SubType = dr[7].ToString()!;
                        result.FieldName = "Description";
                        result.SearchText = TextUtils.GetSubstring(dr[1].ToString()!,0,DataConnection.SearchTextLength, true);
                        
                        results.Add(result);
                    }
                }

                return results;
            }
        }
        
        public static List<ToDo> GetActiveToDos(string connectionString)
        {
            var myTodos = new List<ToDo>();
            ToDo todo;

            var sql = "SELECT td.Id, td.Description, td.CreationDate, td.StartDate, td.DueDate, td.PercentDone, " +
                "tdt.Id, tdt.Description, tds.Id, tds.Description,  " +
                "p1.Id, p1.FirstName, p1.LastName, " +
                "p2.Id Id2, p2.FirstName fn, p2.LastName ln, td.LastUpdatedDate " +
                "FROM ToDo td, ToDoType tdt, ToDoStatus tds, Person p1, Person p2 " +
                "WHERE td.ToDoTypeId = tdt.Id " +
                "AND td.ToDoStatusId = tds.Id " +
                "AND td.AssignerId = p1.Id " +
                "AND tds.Description NOT IN ('Complete','Abandoned') " +
                "AND td.AssigneeId = p2.Id ORDER BY td.DueDate ASC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        todo = new ToDo();

                        todo.Id = Convert.ToInt64(dr[0].ToString());
                        todo.Description = dr[1].ToString();
                        todo.CreationDate = Convert.ToDateTime(dr[2].ToString());
                        todo.StartDate = Convert.ToDateTime(dr[3].ToString());
                        todo.DueDate = DateTime.Parse(dr[4].ToString());
                        todo.PercentDone = Convert.ToInt16(dr[5].ToString());

                        todo.ToDoType = new ToDoType();
                        todo.ToDoType.Id = Convert.ToInt64(dr[6].ToString());
                        todo.ToDoType.Description = dr[7].ToString()!;

                        todo.ToDoStatus = new ToDoStatus();
                        todo.ToDoStatus.Id = Convert.ToInt64(dr[8].ToString());
                        todo.ToDoStatus.Description = dr[9].ToString()!;

                        todo.Assigner = new Person();
                        todo.Assigner.Id = Convert.ToInt64(dr[10].ToString());
                        todo.Assigner.FirstName = dr[11].ToString();
                        todo.Assigner.LastName = dr[12].ToString();

                        todo.Assignee = new Person();
                        todo.Assignee.Id = Convert.ToInt64(dr[13].ToString());
                        todo.Assignee.FirstName = dr[14].ToString();
                        todo.Assignee.LastName = dr[15].ToString();
                        
                        todo.LastUpdatedDate = Convert.ToDateTime(dr[16].ToString());

                        myTodos.Add(todo);
                    }
                }

                return myTodos;
            }
        }

        public static List<ToDo> GetUpcomingToDos(string connectionString, long daysInAdvance)
        {
            var myTodos = new List<ToDo>();
            ToDo todo;

            var sql = "SELECT td.Id, td.Description, td.CreationDate, td.StartDate, td.DueDate, td.PercentDone, " +
                "tdt.Id, tdt.Description, tds.Id, tds.Description,  " +
                "p1.Id, p1.FirstName, p1.LastName, " +
                "p2.Id Id2, p2.FirstName fn, p2.LastName ln, td.LastUpdatedDate " +
                "FROM ToDo td, ToDoType tdt, ToDoStatus tds, Person p1, Person p2 " +
                "WHERE td.ToDoTypeId = tdt.Id " +
                "AND td.ToDoStatusId = tds.Id " +
                "AND td.AssignerId = p1.Id " +
                "AND ((td.DueDate BETWEEN @startDate AND @upcomingDate) OR (td.DueDate < @startDate AND td.PercentDone < 100)) " +
                "AND td.AssigneeId = p2.Id ORDER BY td.DueDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new SqliteParameter("@startDate", DateTime.Today.AddDays(-1)));
                    command.Parameters.Add(new SqliteParameter("@upcomingDate", DateTime.Today.AddDays(daysInAdvance)));
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        todo = new ToDo();

                        todo.Id = Convert.ToInt64(dr[0].ToString());
                        todo.Description = dr[1].ToString();
                        todo.CreationDate = Convert.ToDateTime(dr[2].ToString());
                        todo.StartDate = Convert.ToDateTime(dr[3].ToString());
                        todo.DueDate = DateTime.Parse(dr[4].ToString());
                        todo.PercentDone = Convert.ToInt16(dr[5].ToString());

                        todo.ToDoType = new ToDoType();
                        todo.ToDoType.Id = Convert.ToInt64(dr[6].ToString());
                        todo.ToDoType.Description = dr[7].ToString()!;

                        todo.ToDoStatus = new ToDoStatus();
                        todo.ToDoStatus.Id = Convert.ToInt64(dr[8].ToString());
                        todo.ToDoStatus.Description = dr[9].ToString()!;

                        todo.Assigner = new Person();
                        todo.Assigner.Id = Convert.ToInt64(dr[10].ToString());
                        todo.Assigner.FirstName = dr[11].ToString();
                        todo.Assigner.LastName = dr[12].ToString();

                        todo.Assignee = new Person();
                        todo.Assignee.Id = Convert.ToInt64(dr[13].ToString());
                        todo.Assignee.FirstName = dr[14].ToString();
                        todo.Assignee.LastName = dr[15].ToString();
                        
                        todo.LastUpdatedDate = Convert.ToDateTime(dr[16].ToString());

                        if (todo.ToDoStatus.Description != "Abandoned")
                        {
                            myTodos.Add(todo);
                        }
                    }
                }

                return myTodos;
            }
        }
        
        public static bool Insert(ToDo todo)
        {
            try
            {
                if (todo != null && todo.ToDoStatus.Description == "Complete")
                    todo.PercentDone = 100;

                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO ToDo (Description, StartDate, DueDate, CreationDate, ToDoTypeId, AssignerId, AssigneeId, ToDoStatusId, PercentDone, LastUpdatedDate) " +
                        "VALUES(@Description, @StartDate, @DueDate, CURRENT_DATE, @ToDoTypeId, @AssignerId, @AssigneeId, @ToDoStatusId, @PercentDone, CURRENT_TIMESTAMP);";

                    return (db.Execute(sqlQuery, todo) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Update(ToDo todo)
        {
            try
            {
                if (todo != null && todo.ToDoStatus.Description == "Complete")
                    todo.PercentDone = 100;

                if (todo != null)
                {
                    using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                    {
                        string sqlQuery =
                            "UPDATE ToDo SET Description = @Description, StartDate = @StartDate, DueDate = @DueDate, ToDoTypeId = @ToDoTypeId, " +
                            "AssignerId = @AssignerId, AssigneeId = @AssigneeId, ToDoStatusId = @ToDoStatusId, PercentDone = @PercentDone, LastUpdatedDate = CURRENT_TIMESTAMP " +
                            "WHERE Id = @Id;";

                        return (db.Execute(sqlQuery, todo) == 1);
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

        public static bool DoesToDoExist(string? description)
        {
            long count = 0;
            
            var sql = "SELECT td.Id " +
                "FROM ToDo td, ToDoStatus tds " +
                "WHERE td.ToDoStatusId = tds.Id " +
                "AND tds.Description != 'Complete' " +
                "AND td.Description = @description";

            using (IDbConnection connection = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new SqliteParameter("@description", description));
                        
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        count = count + 1;
                    }
                }
            }
            
            return count > 0;
        }
    }
}
