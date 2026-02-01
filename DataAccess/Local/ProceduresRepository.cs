using System.Data;
using Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class ProceduresRepository
{
    public static List<StandardOperatingProcedure> GetAll(string connectionString)
    {
        var myProcedures = new List<StandardOperatingProcedure>();
        StandardOperatingProcedure sop;

        try
        {

            var sql = "SELECT p.Id, p.Category, p.Name, p.Content, p.AuthorId, p1.FirstName, p1.LastName, p.LastUpdated, p.CreationDate, p.EndDate " +
                      "FROM Procedure p, Person p1 " +
                      "WHERE p.AuthorId = p1.Id " +
                      "ORDER BY p.Category, p.Name ASC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        sop = new StandardOperatingProcedure();
                        
                        sop.Id = Convert.ToInt64(dr[0].ToString());
                        sop.Category = dr[1].ToString();
                        sop.Name = dr[2].ToString();
                        sop.Content = dr[3].ToString();
                        
                        sop.Author = new Person();
                        sop.Author.Id = Convert.ToInt64(dr[4].ToString());
                        sop.Author.FirstName = dr[5].ToString();
                        sop.Author.LastName = dr[6].ToString();
                        
                        sop.LastUpdatedDate = Convert.ToDateTime(dr[7].ToString());
                        sop.CreationDate = Convert.ToDateTime(dr[8].ToString());
                        
                        if (dr[9] != DBNull.Value)
                            sop.EndDate = Convert.ToDateTime(dr[9].ToString());

                        myProcedures.Add(sop);
                    }
                }
            }

            return myProcedures;

        }
        catch (Exception)
        {
            return myProcedures;
        }
    }
    
    public static StandardOperatingProcedure GetFromId(string connectionString, long id)
    {
        StandardOperatingProcedure sop = null;

        try
        {

            var sql = "SELECT p.Id, p.Category, p.Name, p.Content, p.AuthorId, p1.FirstName, p1.LastName, p.LastUpdated, p.CreationDate, p.EndDate " +
                      "FROM Procedure p, Person p1 " +
                      "WHERE p.AuthorId = p1.Id AND p.Id = @Id " +
                      "ORDER BY p.Category, p.Name ASC";

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
                        sop = new StandardOperatingProcedure();
                        
                        sop.Id = Convert.ToInt64(dr[0].ToString());
                        sop.Category = dr[1].ToString();
                        sop.Name = dr[2].ToString();
                        sop.Content = dr[3].ToString();
                        
                        sop.Author = new Person();
                        sop.Author.Id = Convert.ToInt64(dr[4].ToString());
                        sop.Author.FirstName = dr[5].ToString();
                        sop.Author.LastName = dr[6].ToString();
                        
                        sop.LastUpdatedDate = Convert.ToDateTime(dr[7].ToString());
                        sop.CreationDate = Convert.ToDateTime(dr[8].ToString());
                        
                        if (dr[9] != DBNull.Value)
                            sop.EndDate = Convert.ToDateTime(dr[9].ToString());
                        
                    }
                }
            }

            return sop;

        }
        catch (Exception)
        {
            return sop;
        }
    }
    
    public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
    {
        var results = new List<SearchResult>();
        
        var sql = "SELECT i.Id, i.Category, i.Name, i.Content, " +
            "i.CreationDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName  " +
            "FROM Procedure i, Person p " +
            "WHERE i.AuthorId = p.Id " +
            "AND lower(i.Category) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
            "ORDER BY i.CreationDate DESC";

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
                    result.AsOfDate = Convert.ToDateTime(dr[6].ToString());
                    result.IsCurrent = true;
                    result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                    result.Entity.Name = "Procedures";
                    result.SubType = dr[1].ToString()!;
                    result.FieldName = "Category";
                    result.SearchText = TextUtils.GetSubstring(dr[1].ToString()!,0,DataConnection.SearchTextLength, true);

                    results.Add(result);
                }
            }
        }

        sql = "SELECT i.Id, i.Category, i.Name, i.Content, " +
                  "i.CreationDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName  " +
                  "FROM Procedure i, Person p " +
                  "WHERE i.AuthorId = p.Id " +
                  "AND lower(i.Name) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY i.CreationDate DESC";

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
                    result.AsOfDate = Convert.ToDateTime(dr[6].ToString());
                    result.IsCurrent = true;
                    result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                    result.Entity.Name = "Procedures";
                    result.SubType = dr[1].ToString()!;
                    result.FieldName = "Name";
                    result.SearchText = TextUtils.GetSubstring(dr[2].ToString()!,0,DataConnection.SearchTextLength, true);

                    results.Add(result);
                }
            }
        }

        sql = "SELECT i.Id, i.Category, i.Name, i.Content, " +
              "i.CreationDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName  " +
              "FROM Procedure i, Person p " +
              "WHERE i.AuthorId = p.Id " +
              "AND lower(i.Content) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
              "ORDER BY i.CreationDate DESC";

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
                    result.AsOfDate = Convert.ToDateTime(dr[6].ToString());
                    result.IsCurrent = true;
                    result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                    result.Entity.Name = "Procedures";
                    result.SubType = dr[1].ToString()!;
                    result.FieldName = "Content";
                    result.SearchText = TextUtils.GetSubstring(dr[3].ToString()!,0,DataConnection.SearchTextLength, true);

                    results.Add(result);
                }
            }

        }
        
        return results;
        
    }
    
    public static bool Insert(StandardOperatingProcedure sop)
    {
        try
        {
            if (sop != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "INSERT INTO Procedure (Category, Name, Content, EndDate, AuthorId, " + 
                        "LastUpdated, CreationDate) " + 
                        "VALUES (@Category, @Name, @Content, @EndDate, @AuthorId, " +
                        "CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

                    return (db.Execute(sqlQuery, sop) == 1);
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
    
    public static bool Update(StandardOperatingProcedure sop)
    {
        try
        {
            if (sop != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE Procedure SET Category = @Category, Name = @Name, Content = @Content, EndDate = @EndDate, " +
                        "AuthorId = @AuthorId, " + 
                        "LastUpdated = CURRENT_TIMESTAMP " +
                        "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, sop) == 1);
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
    
    public static bool Delete(StandardOperatingProcedure sop)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "UPDATE Procedure SET EndDate = @EndDate WHERE Id = @Id;";

                return (db.Execute(sqlQuery, sop) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}

