using System.Data;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace Services;

public static class DbMigrationService
{
    /// <summary>
    /// Migrate the local SQLite database to a postgresql server.
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
                        var pgSql = @"INSERT INTO CommentType (code,description,creationdate,startdate,enddate,authorid) VALUES(" + 
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    "'" + dr[4].ToString() + "'," +
                                    ConvertToDateTime(dr,5) + "," +
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
                        var pgSql = @"INSERT INTO Event (description,eventtypeid,assignerid,assigneeid,creationdate,startdate,enddate,frequencyid,todotrigger,warningdays,lasttriggerdate,lastupdateddate) VALUES(" + 
                                    "'" + dr[1].ToString().Replace("'","''") + "'," +
                                    dr[2].ToString() + "," +
                                    dr[3].ToString() + "," +
                                    dr[4].ToString() + "," +
                                    "'" + dr[5].ToString() + "'," +
                                    "'" + dr[6].ToString() + "'," +
                                    ConvertToDateTime(dr,7) + "," +
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
                        var pgSql = @"INSERT INTO EventType (description,creationdate,startdate,enddate) VALUES(" + 
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    ConvertToDateTime(dr,4) +
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
                        var pgSql = @"INSERT INTO FeedSource (code,description,creationdate,startdate,enddate) VALUES(" + 
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    "'" + dr[3].ToString() + "'," +
                                    "'" + dr[4].ToString() + "'," +
                                    ConvertToDateTime(dr,5) +
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
                        var pgSql = @"INSERT INTO Frequency (code,description,authorid,creationdate,startdate,enddate) VALUES(" + 
                                    "'" + dr[1].ToString() + "'," +
                                    "'" + dr[2].ToString() + "'," +
                                    dr[3].ToString() + "," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'," +
                                    ConvertToDateTime(dr,6) +
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
                        var pgSql = @"INSERT INTO GardenBed (code,description,locationid,permaculturezone,gardenbedtypeid,authorid,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    dr[3].ToString() + "," +
                                    dr[4].ToString() + "," +
                                    dr[5].ToString() + "," +
                                    dr[6].ToString() + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToDateTime(dr,9) +
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
                        var pgSql = @"INSERT INTO GardenBedType (code,description,authorid,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    dr[3].ToString() + "," +
                                    "'" + dr[4].ToString() + "'," +
                                    "'" + dr[5].ToString() + "'," +
                                    ConvertToDateTime(dr,6) +
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
                        var pgSql = @"INSERT INTO InventoryGroup (description,creationdate,startdate,enddate,authorid) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
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
                        var pgSql = @"INSERT INTO InventoryType (description,creationdate,startdate,enddate,authorid) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
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
                        var pgSql = @"INSERT INTO Inventory (description,inventorygroupid,inventorytypeid,originalvalue,currentvalue,brand,notes,creationdate,startdate,enddate,lastupdated,authorid,room,quantity,forsale) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToNumeric(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToText(dr,9) + "," +
                                    ConvertToDateTime(dr,10) + "," +
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
                        var pgSql = @"INSERT INTO Observation " + 
                                    "(comment,creationdate,startdate,enddate,commenttypeid,authorid)  VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
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
                        var pgSql = @"INSERT INTO Person (firstname,lastname,creationdate,startdate,enddate,company,email,comment,phone) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
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
                        var pgSql = @"INSERT INTO PersonObservation (personid,comment,creationdate,startdate,enddate,commenttypeid,authorid) VALUES(" + 
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
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
                        var pgSql = @"INSERT INTO Plant (code,description,startdate,enddate,creationdate,comment,family,url,authorid) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
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
                        var pgSql = @"INSERT INTO Planting (description,plantid,seedpacketid,gardenbedid,creationdate,startdate,enddate,yieldrating,authorid,comment,plantingstateid) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToDateTime(dr,6) + "," +
                                    ConvertToDateTime(dr,7) + "," +
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
        
        // PlantingObservation
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM PlantingObservation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO PlantingObservation (plantingid,comment,creationdate,startdate,enddate,commenttypeid,authorid) VALUES(" + 
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
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
        
        // PlantingState
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM PlantingState;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO PlantingState (code,description,creationdate,startdate,enddate) VALUES(" +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + 
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
        
        // Seasonality
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Seasonality;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Seasonality (code,description,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToDateTime(dr,5) +
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
        
        // SeedPacket
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM SeedPacket;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO SeedPacket (description,instructions,daystoharvest,creationdate,startdate,enddate,plantid,vendorid,authorid,code,generations,seasonalityid,species) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToDateTime(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + "," +
                                    ConvertToNumeric(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) + "," +
                                    ConvertToNumeric(dr,11) + "," +
                                    ConvertToNumeric(dr,12) + "," +
                                    ConvertToText(dr,13) + 
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
        
        // SeedPacketObservation
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM SeedPacketObservation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO SeedPacketObservation (seedpacketid,comment,creationdate,startdate,enddate,commenttypeid,authorid) VALUES(" + 
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
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
        
        // Settings
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Settings;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Settings VALUES(" + 
                                    ConvertToText(dr,0) + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + 
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
        
        // ToDo
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM ToDo;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDo (description,todotypeid,assignerid,assigneeid,creationdate,startdate,todostatusid,duedate,percentdone,lastupdateddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) +  
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
        
        // ToDoStatus
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM ToDoStatus;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDoStatus (description,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) +
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
        
        // ToDoType
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM ToDoType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDoType (description,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) +
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
        
        // Vendor
        try
        {
            using (IDbConnection connection = new SqliteConnection(localConnectionString))
            {
                var sql = "SELECT * FROM Vendor;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Vendor (code,description,rating,creationdate,startdate,enddate) VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToDateTime(dr,6) +
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

    /// <summary>
    /// Migrate the postgresql server data to the local SQLite database.
    /// </summary>
    /// <returns></returns>
    public static string MigrateServerToLocal(string localConnectionString, string serverConnectionString)
    {
        // first step is to run the creation script
        Console.WriteLine("Migrating to Local DB");
        
        // drop/setup the tables
        DataAccess.Local.HomesteaderRepository.DatabaseExists();
        
        // now migrate the tables
        
        // CommentType
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,5) + "," +
                                    dr[6].ToString() + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,7) + "," +
                                    dr[8].ToString() + "," +
                                    (dr[9].ToString()=="1") + "," + //boolean conversion
                                    dr[10].ToString() + "," +
                                    "'" + dr[11].ToString() + "'," +
                                    "'" + dr[12].ToString() + "'" +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,4) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,6) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,9) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,6) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToNumeric(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToNumeric(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,10) + "," +
                                    ConvertToText(dr,11) + "," +
                                    ConvertToNumeric(dr,12) + "," +
                                    ConvertToText(dr,13) + "," +
                                    ConvertToNumeric(dr,14) + "," +
                                    ConvertToBoolean(dr,15) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM Observation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Observation " + 
                                    "(comment,creationdate,startdate,enddate,commenttypeid,authorid)  VALUES(" + 
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToNumeric(dr,5) + "," +
                                    ConvertToNumeric(dr,6) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToText(dr,9) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToNumeric(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToText(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
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
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToDateTime(dr,6) + "," +
                                    ConvertToDateTime(dr,7) + "," +
                                    ConvertToNumeric(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) + "," +
                                    ConvertToNumeric(dr,11) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // PlantingObservation
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM PlantingObservation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO PlantingObservation VALUES(" + dr[0].ToString() + "," +
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToNumeric(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // PlantingState
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM PlantingState;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO PlantingState VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Seasonality
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM Seasonality;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Seasonality VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToDateTime(dr,5) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // SeedPacket
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM SeedPacket;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO SeedPacket VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToDateTime(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + "," +
                                    ConvertToNumeric(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) + "," +
                                    ConvertToNumeric(dr,11) + "," +
                                    ConvertToNumeric(dr,12) + "," +
                                    ConvertToText(dr,13) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // SeedPacketObservation
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM SeedPacketObservation;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO SeedPacketObservation VALUES(" + dr[0].ToString() + "," +
                                    ConvertToNumeric(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToDateTime(dr,3) + "," +
                                    ConvertToDateTime(dr,4) + "," +
                                    ConvertToDateTime(dr,5) + "," +
                                    ConvertToNumeric(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Settings
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM Settings;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Settings VALUES(" + 
                                    ConvertToText(dr,0) + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + 
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // ToDo
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM ToDo;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDo VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToNumeric(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToNumeric(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToText(dr,6) + "," +
                                    ConvertToNumeric(dr,7) + "," +
                                    ConvertToText(dr,8) + "," +
                                    ConvertToNumeric(dr,9) + "," +
                                    ConvertToText(dr,10) +  
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // ToDoStatus
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM ToDoStatus;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDoStatus VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // ToDoType
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM ToDoType;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO ToDoType VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToText(dr,3) + "," +
                                    ConvertToDateTime(dr,4) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        // Vendor
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(serverConnectionString))
            {
                var sql = "SELECT * FROM Vendor;";
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pgSql = @"INSERT INTO Vendor VALUES(" + dr[0].ToString() + "," +
                                    ConvertToText(dr,1) + "," +
                                    ConvertToText(dr,2) + "," +
                                    ConvertToNumeric(dr,3) + "," +
                                    ConvertToText(dr,4) + "," +
                                    ConvertToText(dr,5) + "," +
                                    ConvertToDateTime(dr,6) +
                                    ")";
                        Console.WriteLine(pgSql);
                        RunLocalSql(localConnectionString, pgSql);
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
    
    /// <summary>
    /// Runs a sql command on the Postgresql server.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="sql"></param>
    private static void RunServerSql(string connectionString, string sql)
    {
        using IDbConnection connection = new NpgsqlConnection(connectionString);
        connection.Open();
        using IDbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        var dr = command.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Runs a sql command on the local SQLite database.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="sql"></param>
    private static void RunLocalSql(string connectionString, string sql)
    {
        using IDbConnection connection = new SqliteConnection(connectionString);
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
    
    private static string ConvertToDateTime(IDataReader dr, int index)
    {
        // make sure we don't go over 9999 as the year
        var myDateTime = Convert.ToDateTime(dr[index].ToString());

        if (myDateTime.Year > 9999)
        {
            myDateTime = new DateTime(9999, myDateTime.Month, myDateTime.Day);
        }
            
        return "'" + myDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
    }
}