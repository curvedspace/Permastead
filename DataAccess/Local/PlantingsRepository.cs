using Dapper;
using Microsoft.Data.Sqlite;

using System.Data;

using Models;


namespace DataAccess.Local;

public class PlantingsRepository
{
    public static List<Planting> GetAllPlantings(string connectionString)
    {
        var plantings = new List<Planting>();
        Planting planting;

        var sql = "SELECT p.Id, p.Description, p3.Id plantId, p3.Description plant, " + 
        "p.CreationDate, p.StartDate, p2.Id, p2.FirstName, p2.LastName, p.Comment,  " + 
        "gb.Id, gb.Code as gbcode, gb.Description as gbdesc,  " + 
        "sp.Id , sp.DaysToHarvest, sp.Description as PacketDesc,  " + 
        "v.Id, v.Code, v.Description as VendorDesc, p.EndDate, p.YieldRating, ps.Id, ps.Code, ps.Description " + 
        "FROM Planting p, Person p2, Plant p3, GardenBed gb, SeedPacket sp, Vendor v, PlantingState ps  " + 
        "WHERE p.AuthorId = p2.Id AND p.GardenBedId = gb.Id  " + 
        "AND p.PlantId = p3.Id AND p.PlantingStateId = ps.Id " + 
        "AND p.SeedPacketId = sp.Id AND sp.VendorId = v.Id ORDER BY p.StartDate ";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    planting = new Planting();

                    planting.Id = Convert.ToInt64(dr[0].ToString());
                    planting.Description = dr[1].ToString();
                    planting.Plant.Id = Convert.ToInt64(dr[2].ToString());
                    planting.Plant.Description = dr[3].ToString();
                    planting.CreationDate = Convert.ToDateTime(dr[4].ToString());
                    planting.StartDate = Convert.ToDateTime(dr[5].ToString());

                    planting.Author = new Person();
                    planting.Author.Id = Convert.ToInt64(dr[6].ToString());
                    planting.Author.FirstName = dr[7].ToString();
                    planting.Author.LastName = dr[8].ToString();

                    planting.Comment = dr[9].ToString()!;
                    
                    planting.Bed.Id = Convert.ToInt64(dr[10].ToString());
                    planting.Bed.Code = dr[11].ToString();
                    planting.Bed.Description = dr[12].ToString();
                    
                    planting.SeedPacket.Id = Convert.ToInt64(dr[13].ToString());
                    planting.SeedPacket.DaysToHarvest = Convert.ToInt32(dr[14].ToString());
                    planting.SeedPacket.Description = dr[15].ToString();
                    
                    planting.SeedPacket.Vendor.Id = Convert.ToInt64(dr[16].ToString());
                    planting.SeedPacket.Vendor.Code = dr[17].ToString();
                    planting.SeedPacket.Vendor.Description = dr[18].ToString();
                    
                    planting.EndDate = Convert.ToDateTime(dr[19].ToString());
                    planting.YieldRating = Convert.ToInt64(dr[20].ToString());
                    
                    planting.State.Id = Convert.ToInt64(dr[21].ToString());
                    planting.State.Code = dr[22].ToString();
                    planting.State.Description = dr[23].ToString();
                    
                    plantings.Add(planting);
                }
            }

            return plantings;
        }
    }

    public static Planting GetPlantingFromId(string connectionString, long id)
    {        
        Planting planting = new Planting();

        var sql = "SELECT p.Id, p.Description, p3.Id plantId, p3.Description plant, " +
        "p.CreationDate, p.StartDate, p2.Id, p2.FirstName, p2.LastName, p.Comment,  " +
        "gb.Id, gb.Code as gbcode, gb.Description as gbdesc,  " +
        "sp.Id , sp.DaysToHarvest, sp.Description as PacketDesc,  " +
        "v.Id, v.Code, v.Description as VendorDesc, p.EndDate, p.YieldRating , ps.Id, ps.Code, ps.Description " + 
        "FROM Planting p, Person p2, Plant p3, GardenBed gb, SeedPacket sp, Vendor v , PlantingState ps  " + 
        "WHERE p.AuthorId = p2.Id AND p.GardenBedId = gb.Id  " +
        "AND p.PlantId = p3.Id AND p.PlantingStateId = ps.Id " + 
        "AND p.Id = @id " + 
        "AND p.SeedPacketId = sp.Id AND sp.VendorId = v.Id ORDER BY p.StartDate ";

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
                    planting = new Planting();

                    planting.Id = Convert.ToInt64(dr[0].ToString());
                    planting.Description = dr[1].ToString();
                    planting.Plant.Id = Convert.ToInt64(dr[2].ToString());
                    planting.Plant.Description = dr[3].ToString();
                    planting.CreationDate = Convert.ToDateTime(dr[4].ToString());
                    planting.StartDate = Convert.ToDateTime(dr[5].ToString());

                    planting.Author = new Person();
                    planting.Author.Id = Convert.ToInt64(dr[6].ToString());
                    planting.Author.FirstName = dr[7].ToString();
                    planting.Author.LastName = dr[8].ToString();

                    planting.Comment = dr[9].ToString()!;

                    planting.Bed.Id = Convert.ToInt64(dr[10].ToString());
                    planting.Bed.Code = dr[11].ToString();
                    planting.Bed.Description = dr[12].ToString();

                    planting.SeedPacket.Id = Convert.ToInt64(dr[13].ToString());
                    planting.SeedPacket.DaysToHarvest = Convert.ToInt32(dr[14].ToString());
                    planting.SeedPacket.Description = dr[15].ToString();

                    planting.SeedPacket.Vendor.Id = Convert.ToInt64(dr[16].ToString());
                    planting.SeedPacket.Vendor.Code = dr[17].ToString();
                    planting.SeedPacket.Vendor.Description = dr[18].ToString();

                    planting.EndDate = Convert.ToDateTime(dr[19].ToString());
                    planting.YieldRating = Convert.ToInt64(dr[20].ToString());
                    
                    planting.State.Id = Convert.ToInt64(dr[21].ToString());
                    planting.State.Code = dr[22].ToString();
                    planting.State.Description = dr[23].ToString();

                }
            }

            return planting;
        }
    }

    public static List<long> GetAllActivePlantingIds(string connectionString)
    {
        var plantings = new List<long>();

        var sql = "SELECT DISTINCT sp.Id FROM Planting p, SeedPacket sp WHERE p.SeedPacketId = sp.Id AND p.EndDate > Date(); ";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                var dr = command.ExecuteReader();
                long currentId;

                while (dr.Read())
                {

                    currentId = Convert.ToInt64(dr[0].ToString());
                    
                    if (!plantings.Contains(currentId))
                        plantings.Add(currentId);
                }
            }

            return plantings;
        }
    }

    public static bool Insert(Planting planting)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO Planting (Description, PlantId, SeedPacketId, GardenBedId, StartDate, EndDate, CreationDate, YieldRating, AuthorId, PlantingStateId, Comment) " +
                    "VALUES(@Description, @PlantId, @SeedPacketId, @BedId, @StartDate, @EndDate, CURRENT_DATE, @YieldRating, @AuthorId, @PlantingStateId, @Comment);";

                return (db.Execute(sqlQuery, planting) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(Planting planting)
    {
        try
        {
            if (planting != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE Planting SET Description = @Description, StartDate = @StartDate, EndDate = @EndDate, PlantId = @PlantId, " +
                        "SeedPacketId = @SeedPacketId, GardenBedId = @BedId, AuthorId = @AuthorId, YieldRating = @YieldRating, PlantingStateId = @PlantingStateId, " +
                        "Comment = @Comment " +
                        "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, planting) == 1);
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