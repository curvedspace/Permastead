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
    
    public static GardenBed GetFromId(string conn, long id)
    {
        GardenBed bed = new GardenBed();
        
        try
        {
            using (IDbConnection db = new SqliteConnection(conn))
            {
                string sql = "SELECT Id, Code, Description, PermacultureZone, GardenBedTypeId, " + 
                             "CreationDate, StartDate, EndDate, AuthorId FROM GardenBed WHERE Id = @Id;";
                
                
                using (IDbConnection connection = new SqliteConnection(conn))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.Parameters.Add(new SqliteParameter("@id", id));
                    
                        var dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            bed = new GardenBed();
                            
                            bed.Id = Convert.ToInt64(dr[0].ToString());
                            bed.Code = dr[1].ToString()!;
                            bed.Description = dr[2].ToString()!;
                            bed.PermacultureZone = Convert.ToInt64(dr[3].ToString());

                            bed.CreationDate = Convert.ToDateTime(dr[4].ToString());
                            bed.StartDate = Convert.ToDateTime(dr[5].ToString());
                            bed.EndDate = Convert.ToDateTime(dr[6].ToString());

                            bed.Author = new Person();
                            bed.Author.Id = Convert.ToInt64(dr[7].ToString());
                            
                        }
                    }
                }

                return bed;
            }
        }
        catch
        {
            return new GardenBed();
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