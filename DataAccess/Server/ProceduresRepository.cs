using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

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

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
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
    
    public static bool Insert(StandardOperatingProcedure sop)
    {
        try
        {
            if (sop != null)
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
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
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
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
}