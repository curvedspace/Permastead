using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;
using Npgsql;

namespace Services;

public static class DbMigrationService
{
    /// <summary>
    /// Migrate the local SqLite database to a a postegres server.
    /// </summary>
    /// <returns></returns>
    public static string MigrateLocalToServer(string localConnectionString, string serverConnectionString)
    {
        // first step is to run the creation script
        Console.WriteLine("Migrating to Server DB");
        
        // drop/setup the tables
        DataAccess.Server.HomesteaderRepository.DatabaseExists();
        
        // now migrate the tables
        
        // CommentType
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM CommentType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO CommentType VALUES(" + dr[0].ToString() + "," +
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'," +
                                    dr[6].ToString() + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Event
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Event;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Event VALUES(" + dr[0].ToString() + "," +
                                    "'" + dr[1].ToString().Replace("'","''") + "'," +
                                    dr[2].ToString() + "," +
                                    dr[3].ToString() + "," +
                                    dr[4].ToString() + "," +
                                    "'" + dr[5].ToString() + "'," +
                                    "'" + dr[6].ToString() + "'," +
                                    "'" + dr[7].ToString() + "'," +
                                    dr[8].ToString() + "," +
                                    (dr[9].ToString()=="1") + "," + //boolean conversion
                                    dr[10].ToString() + "," +
                                    "'" + dr[11].ToString() + "'," +
                                    "'" + dr[12].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // EventType
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM EventType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO EventType VALUES(" + dr[0].ToString() + "," +
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    "'" + dr[4].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // FeedSource
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM FeedSource;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO FeedSource VALUES(" + dr[0].ToString() + "," +
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Frequency
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Frequency;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Frequency VALUES(" + dr[0].ToString() + "," +
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    dr[3].ToString() + "," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'," +
                                    "'" + dr[6].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // GardenBed
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM GardenBed;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO GardenBed VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    dr[3].ToString() + "," +
                                    dr[4].ToString() + "," +
                                    dr[5].ToString() + "," +
                                    dr[6].ToString() + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToText(dr,9) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // GardenBedType
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM GardenBedType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO GardenBedType VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    dr[3].ToString() + "," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'," +
                                    "'" + dr[6].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // InventoryGroup
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM InventoryGroup;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO InventoryGroup VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToNumeric(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // InventoryType
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM InventoryType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO InventoryType VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToNumeric(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Inventory
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Inventory;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Inventory VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToNumeric(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToText(dr,9) + "," +
                                    ConvertToText(dr,10) + "," +
                                    ConvertToText(dr,11) + "," +
                                    ConvertToNumeric(dr,12) + "," +
                                    ConvertToText(dr,13) + "," +
                                    ConvertToNumeric(dr,14) + "," +
                                    ConvertToBoolean(dr,15) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Observation
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Observation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Observation VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToNumeric(dr,5) + "," +
                                    ConvertToNumeric(dr,6) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Person
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Person;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Person VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToText(dr,9) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // PersonObservation
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM PersonObservation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO PersonObservation VALUES(" + dr[0].ToString() + "," +
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToNumeric(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Plant
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Plant;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Plant VALUES(" + 
                                    ConvertToNumeric(dr,0) + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Planting
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Planting;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Planting VALUES(" + 
                                    ConvertToNumeric(dr,0) + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToNumeric(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) + "," +
                                    ConvertToNumeric(dr,11) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunServerSql(serverConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Console.WriteLine("Completed migration to Server DB");
        
        return "";
    }

    public static void RunServerSql(string connectionString, string sql)
    {
        using IDbConnection connection = new NpgsqlConnection(connectionString);
        connection.Open();
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        var dr = command.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Migrate the postgres server to a local SQLite one.
    /// </summary>
    /// <returns></returns>
    public static string MigrateServerToLocal()
    {
        return "";
    }

    private static string ConvertToText(IDataReader dr, int index)
    {
        // escape out any single quotes in the text
        return "'" + dr[index].ToString().Replace("'", "''") + "'";
    }
    
    private static string ConvertToNumeric(IDataReader dr, int index)
    {
        // escape out any single quotes in the text
        return dr[index].ToString();
    }
    
    private static bool ConvertToBoolean(IDataReader dr, int index)
    {
        // escape out any single quotes in the text
        return (dr[index].ToString() == "1");
    }
    
}