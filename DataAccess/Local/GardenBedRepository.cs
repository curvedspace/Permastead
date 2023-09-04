using Dapper;

using Models;

using Microsoft.Data.Sqlite;
using System.Data;


namespace DataAccess.Local;

public class GardenBedRepository
{
    public static List<GardenBed> GetAll(string conn)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sqlQuery = "SELECT * FROM GardenBed ORDER BY Description;";

                return db.Query<GardenBed>(sqlQuery).ToList();
            }
        }
        catch
        {
            return new List<GardenBed>();
        }
    }
    
    public static bool Insert(GardenBed gb)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO GardenBed (Code, Description, LocationId, PermacultureZone, GardenBedTypeId, AuthorId, CreationDate, StartDate, EndDate) " +
                                  "VALUES(@Code, @Description, @LocationId, @PermacultureZone, @GardenBedTypeId, @AuthorId, CURRENT_DATE, @StartDate, @EndDate);";
        
                return (db.Execute(sqlQuery, gb) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(GardenBed gb)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "UPDATE GardenBed SET Code = @Code, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, " +
                                  "PermacultureZone = @PermacultureZone, LocationId = @LocationId, GardenBedTypeId = @GardenBedTypeId, AuthorId = @AuthorId " + 
                                  "WHERE Id = @Id;";

                return (db.Execute(sqlQuery, gb) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}