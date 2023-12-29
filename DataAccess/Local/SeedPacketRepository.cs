using Dapper;

using Models;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Local;

public class SeedPacketRepository
{
    public static SeedPacket? GetFromId(string connectionString, long id)
    {
        try
        {
            SeedPacket packet = new SeedPacket();
            
            //get current plantings
            var currentPackets = PlantingsRepository.GetAllActivePlantingIds(connectionString);
            
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                string sqlQuery =
                    "SELECT sp.Id, sp.Description, sp.Instructions, sp.DaysToHarvest, sp.CreationDate, sp.StartDate, sp.EndDate," +
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description, sp.Code, sp.Generations, " +
                    "s.Id, s.Code, s.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2, Seasonality s " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND sp.AuthorId = p.Id AND s.Id = sp.SeasonalityId AND sp.Id = @id";
                
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        packet = new SeedPacket();

                        packet.Id = Convert.ToInt64(dr[0].ToString());
                        packet.Description = dr[1].ToString();
                        packet.Instructions = dr[2].ToString();
                        packet.DaysToHarvest = Convert.ToInt32(dr[3].ToString());
                        packet.CreationDate = Convert.ToDateTime(dr[4].ToString());
                        packet.StartDate = Convert.ToDateTime(dr[5].ToString());
                        packet.EndDate = Convert.ToDateTime(dr[6].ToString());

                        packet.Vendor.Id = Convert.ToInt64(dr[7].ToString());
                        packet.Vendor.Code = dr[8].ToString();
                        packet.Vendor.Description = dr[9].ToString();

                        packet.Author = new Person();
                        packet.Author.Id = Convert.ToInt64(dr[10].ToString());
                        packet.Author.FirstName = dr[11].ToString();
                        packet.Author.LastName = dr[12].ToString();

                        packet.Plant!.Id = Convert.ToInt64(dr[13].ToString());
                        packet.Plant.Code = dr[14].ToString();
                        packet.Plant.Description = dr[15].ToString();
                        
                        packet.Code = dr[16].ToString();
                        packet.Generations = Convert.ToInt64(dr[17].ToString());
                        
                        packet.Seasonality!.Id = Convert.ToInt64(dr[18].ToString());
                        packet.Seasonality.Code = dr[19].ToString();
                        packet.Seasonality.Description = dr[20].ToString();

                        packet.IsPlanted = currentPackets.Contains(packet.Id);
                        
                    }
                }
            }

            return packet;
        }
        catch
        {
            return null;
        }
    }

    public static bool Insert(SeedPacket seedPacket)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO SeedPacket (Code, Description, Instructions, DaysToHarvest, CreationDate, StartDate, EndDate, PlantId, VendorId, AuthorId, Generations, SeasonalityId) " +
                                  "VALUES(@Code, @Description, @Instructions, @DaysToHarvest, CURRENT_DATE, @StartDate, @EndDate, @PlantId, @VendorId, @AuthorId, @Generations, @SeasonalityId);";

                return (db.Execute(sqlQuery, seedPacket) == 1);
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool Update(SeedPacket seedPacket)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "UPDATE SeedPacket SET Code = @Code, Description = @Description, " +
                    "Instructions = @Instructions, DaysToHarvest = @DaysToHarvest, StartDate = @StartDate, " +
                    "EndDate = @EndDate, PlantId = @PlantId, VendorId = @VendorId, AuthorId = @AuthorId, Generations = @Generations, " +
                    "SeasonalityId = @SeasonalityId " +
                    "WHERE Id = @Id;";

                return (db.Execute(sqlQuery, seedPacket) == 1);
            }
        }
        catch
        {
            return false;
        }
    }

    public static List<SeedPacket> GetAllByPlant(string connectionString)
    {
        var packets = new List<SeedPacket>();
        SeedPacket packet;

        //get current plantings
        var currentPackets = PlantingsRepository.GetAllActivePlantingIds(connectionString);

        string sqlQuery = "SELECT sp.Id, sp.Description, sp.Instructions, sp.DaysToHarvest, sp.CreationDate, sp.StartDate, sp.EndDate," +
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description, sp.Code, sp.Generations, " +
                    "s.Id, s.Code, s.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2, Seasonality s " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND s.Id = sp.SeasonalityId AND sp.AuthorId = p.Id ORDER BY p2.Description";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    packet = new SeedPacket();

                    packet.Id = Convert.ToInt64(dr[0].ToString());
                    packet.Description = dr[1].ToString();
                    packet.Instructions = dr[2].ToString();
                    packet.DaysToHarvest = Convert.ToInt32(dr[3].ToString());
                    packet.CreationDate = Convert.ToDateTime(dr[4].ToString());
                    packet.StartDate = Convert.ToDateTime(dr[5].ToString());
                    packet.EndDate = Convert.ToDateTime(dr[6].ToString());

                    packet.Vendor.Id = Convert.ToInt64(dr[7].ToString());
                    packet.Vendor.Code = dr[8].ToString();
                    packet.Vendor.Description = dr[9].ToString();

                    packet.Author = new Person();
                    packet.Author.Id = Convert.ToInt64(dr[10].ToString());
                    packet.Author.FirstName = dr[11].ToString();
                    packet.Author.LastName = dr[12].ToString();

                    packet.Plant!.Id = Convert.ToInt64(dr[13].ToString());
                    packet.Plant.Code = dr[14].ToString();
                    packet.Plant.Description = dr[15].ToString();

                    packet.IsPlanted = currentPackets.Contains(packet.Id);
                    packet.Code = dr[16].ToString();
                    packet.Generations = Convert.ToInt64(dr[17].ToString());
                    
                    packet.Seasonality!.Id = Convert.ToInt64(dr[18].ToString());
                    packet.Seasonality.Code = dr[19].ToString();
                    packet.Seasonality.Description = dr[20].ToString();

                    packets.Add(packet);
                }
            }
        }

        return packets;
    }

    public static List<SeedPacket> GetAllForPlant(string connectionString, long plantId)
    {
        var packets = new List<SeedPacket>();
        SeedPacket packet;

        //get current plantings
        var currentPackets = PlantingsRepository.GetAllActivePlantingIds(connectionString);

        string sqlQuery = "SELECT sp.Id, sp.Description, sp.Instructions, sp.DaysToHarvest, sp.CreationDate, sp.StartDate, sp.EndDate," +
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description, sp.Code, sp.Generations, " +
                    "s.Id, s.Code, s.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2, Seasonality s " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND s.Id = sp.SeasonalityId AND sp.AuthorId = p.Id " + 
                    "AND sp.PlantId = @id " +
                    "ORDER BY p2.Description";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.Parameters.Add(new SqliteParameter("@id", plantId));
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    packet = new SeedPacket();

                    packet.Id = Convert.ToInt64(dr[0].ToString());
                    packet.Description = dr[1].ToString();
                    packet.Instructions = dr[2].ToString();
                    packet.DaysToHarvest = Convert.ToInt32(dr[3].ToString());
                    packet.CreationDate = Convert.ToDateTime(dr[4].ToString());
                    packet.StartDate = Convert.ToDateTime(dr[5].ToString());
                    packet.EndDate = Convert.ToDateTime(dr[6].ToString());

                    packet.Vendor.Id = Convert.ToInt64(dr[7].ToString());
                    packet.Vendor.Code = dr[8].ToString();
                    packet.Vendor.Description = dr[9].ToString();

                    packet.Author = new Person();
                    packet.Author.Id = Convert.ToInt64(dr[10].ToString());
                    packet.Author.FirstName = dr[11].ToString();
                    packet.Author.LastName = dr[12].ToString();

                    packet.Plant!.Id = Convert.ToInt64(dr[13].ToString());
                    packet.Plant.Code = dr[14].ToString();
                    packet.Plant.Description = dr[15].ToString();

                    packet.IsPlanted = currentPackets.Contains(packet.Id);
                    packet.Code = dr[16].ToString();
                    packet.Generations = Convert.ToInt64(dr[17].ToString());

                    packets.Add(packet);
                }
            }
        }

        return packets;
    }
    
    public static List<SeedPacket> GetAllByDescription(string connectionString)
    {
        var packets = new List<SeedPacket>();
        SeedPacket packet;

        //get current plantings
        var currentPackets = PlantingsRepository.GetAllActivePlantingIds(connectionString);

        string sqlQuery = "SELECT sp.Id, sp.Description, sp.Instructions, sp.DaysToHarvest, sp.CreationDate, sp.StartDate, sp.EndDate," +
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description, sp.Code, sp.Generations, " +
                    "s.Id, s.Code, s.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2, Seasonality s " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND s.Id = sp.SeasonalityId AND sp.AuthorId = p.Id ORDER BY sp.Description";

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    packet = new SeedPacket();

                    packet.Id = Convert.ToInt64(dr[0].ToString());
                    packet.Description = dr[1].ToString();
                    packet.Instructions = dr[2].ToString();
                    packet.DaysToHarvest = Convert.ToInt32(dr[3].ToString());
                    packet.CreationDate = Convert.ToDateTime(dr[4].ToString());
                    packet.StartDate = Convert.ToDateTime(dr[5].ToString());
                    packet.EndDate = Convert.ToDateTime(dr[6].ToString());

                    packet.Vendor.Id = Convert.ToInt64(dr[7].ToString());
                    packet.Vendor.Code = dr[8].ToString();
                    packet.Vendor.Description = dr[9].ToString();

                    packet.Author = new Person();
                    packet.Author.Id = Convert.ToInt64(dr[10].ToString());
                    packet.Author.FirstName = dr[11].ToString();
                    packet.Author.LastName = dr[12].ToString();

                    packet.Plant!.Id = Convert.ToInt64(dr[13].ToString());
                    packet.Plant.Code = dr[14].ToString();
                    packet.Plant.Description = dr[15].ToString();
                    packet.Code = dr[16].ToString();
                    packet.Generations = Convert.ToInt64(dr[17].ToString());
                    
                    packet.Seasonality!.Id = Convert.ToInt64(dr[18].ToString());
                    packet.Seasonality.Code = dr[19].ToString();
                    packet.Seasonality.Description = dr[20].ToString();

                    packet.IsPlanted = currentPackets.Contains(packet.Id);

                    packets.Add(packet);
                }
            }
        }

        return packets;
    }
    
    public static bool InsertSeedPacketObservation(string connectionString, SeedPacketObservation spObs)
    {
        var rtnValue = false;

        var sql = "INSERT INTO SeedPacketObservation (SeedPacketId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                  "VALUES($seedPacketId, $comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', $commentTypeId, $authorId) ";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("$seedPacketId", spObs.SeedPacketId);
                command.Parameters.AddWithValue("$comment", spObs.Comment);
                command.Parameters.AddWithValue("$commentTypeId", spObs.CommentType!.Id);
                command.Parameters.AddWithValue("$authorId", spObs.Author!.Id);

                rtnValue = (command.ExecuteNonQuery() == 1);
            }
        }

        return rtnValue;
    }

    public static List<SeedPacketObservation> GetAllObservationsForSeedPacket(string connectionString, long plantingId)
    {
        {
            var myObs = new List<SeedPacketObservation>();
            SeedPacketObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.SeedPacketId " +
                      "FROM SeedPacketObservation o, CommentType ct, Person p " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.SeedPacketId = @Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", plantingId);
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new SeedPacketObservation();
                        
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
                        o.SeedPacket.Id = Convert.ToInt64(dr[10].ToString());
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
    
    public static List<SeedPacketObservation> GetAllSeedPacketObservations(string connectionString)
    {
        {
            var myObs = new List<SeedPacketObservation>();
            SeedPacketObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.SeedPacketId, sp.Description " +
                      "FROM SeedPacketObservation o, CommentType ct, Person p, SeedPacket sp " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.SeedPacketId = sp.Id " +
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
                        o = new SeedPacketObservation();
                        
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
                        o.SeedPacket.Id = Convert.ToInt64(dr[10].ToString());
                        o.SeedPacket.Description = dr[11].ToString();
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
}