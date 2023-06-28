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
    public static SeedPacket? GetFromId(long id)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "SELECT * FROM SeedPacket WHERE Id = @Id;";

                return db.QueryFirstOrDefault<SeedPacket>(sqlQuery, new { Id = id });
            }
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
                string sqlQuery = "INSERT INTO SeedPacket (Description, Instructions, DaysToHarvest, CreationDate, StartDate, EndDate, PlantId, VendorId, AuthorId) " +
                                  "VALUES(@Description, @Instructions, @DaysToHarvest, CURRENT_DATE, @StartDate, @EndDate, @PlantId, @VendorId, @AuthorId);";

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
                string sqlQuery = "UPDATE SeedPacket SET Description = @Description, " +
                    "Instructions = @Instructions, DaysToHarvest = @DaysToHarvest, StartDate = @StartDate, " +
                    "EndDate = @BestByDate, PlantId = @PlantId, VendorId = @VendorId, AuthorId = @AuthorId " +
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
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2 " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND sp.AuthorId = p.Id ORDER BY p2.Description";

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
                    packet.BestByDate = new DateTimeOffset(packet.EndDate);

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
                    "v.Id, v.Code, v.Description, p.Id, p.FirstName, p.LastName, p2.Id, p2.Code, p2.Description " +
                    "FROM SeedPacket sp, Vendor v, Person p, Plant p2 " +
                    "WHERE sp.PlantId = p2.Id AND sp.VendorId = v.Id AND sp.AuthorId = p.Id ORDER BY sp.Description";

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
                    packet.BestByDate = new DateTimeOffset(packet.EndDate);

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

                    packets.Add(packet);
                }
            }
        }

        return packets;
    }

}