using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public static class AnimalTypeRepository
{
    public static List<AnimalType> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM AnimalType ORDER BY Description;";

                return db.Query<AnimalType>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<AnimalType>();
        }
    }
    
    public static bool Insert(AnimalType at)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO AnimalType (Code, Description, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate);";

                return (db.Execute(sqlQuery, at) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(AnimalType at)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery =
                    "UPDATE AnimalType SET Code = @Code, Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
                    "WHERE Id = @Id;";

                return (db.Execute(sqlQuery, at) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}