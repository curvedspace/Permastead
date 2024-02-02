using System.Data;
using System.Runtime.InteropServices;

using Dapper;

using Microsoft.Data.Sqlite;
using Models;


namespace DataAccess
{
    public static class DataConnection 
    {
        public static string GetLocalDataSource()
        {
            return @"data source=" + GetDefaultDatabaseLocation() + ";";
        }
        
        public static string GetServerDataSource()
        {
            return GetServerConnectionString();
        }

        public static string GetDefaultDatabaseLocation()
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string dbLocation = "";
            
            //try to read in a db_location.txt file
            FileInfo dbLoc;
            if (isWindows)
            {
                dbLoc = new FileInfo(userFolder + @"\.config\permastead\db_location.txt");
            }
            else
            {
                //linux or macos
                dbLoc = new FileInfo(userFolder + @"/.config/permastead/db_location.txt");
            }
            
            if (dbLoc.Exists)
            {
                //read in db file location from this file
                dbLocation = File.ReadAllText(dbLoc.FullName);
            }

            if (string.IsNullOrEmpty(dbLocation))
            {
                if (isWindows)
                {
                    dbLocation = userFolder + @"\.config\permastead\permastead.db";
                }
                else
                {
                    //linux or macos
                    dbLocation = userFolder + @"/.config/permastead/permastead.db";
                }
            }

            return dbLocation;

        }
        
        public static string GetServerConnectionString()
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string dbLocation = "";
            
            //try to read in a db_location.txt file
            FileInfo dbLoc;
            if (isWindows)
            {
                dbLoc = new FileInfo(userFolder + @"\.config\permastead\pg_connection_string.txt");
            }
            else
            {
                //linux or macos
                dbLoc = new FileInfo(userFolder + @"/.config/permastead/pg_connection_string.txt");
            }
            
            if (dbLoc.Exists)
            {
                //read in db file location from this file
                dbLocation = File.ReadAllText(dbLoc.FullName);
            }

            return dbLocation;

        }

        public static long GetCurrentUserId()
        {
            long currentUserId = 0;
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            
            //try to read in a current_user.txt file
            FileInfo fi;
            if (isWindows)
            {
                fi = new FileInfo(userFolder + @"\.config\permastead\current_user.txt");
            }
            else
            {
                //linux or macos
                fi = new FileInfo(userFolder + @"/.config/permastead/current_user.txt");
            }
            
            if (fi.Exists)
            {
                //read in db file location from this file
                try
                {
                    currentUserId = Convert.ToInt64(File.ReadAllText(fi.FullName));
                }
                catch (Exception e)
                {
                    currentUserId = 0;
                }
            }

            return currentUserId;
        }
        
        public static bool SetCurrentUserId(long userId)
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            
            //try to read in a current_user.txt file
            FileInfo fi;
            if (isWindows)
            {
                fi = new FileInfo(userFolder + @"\.config\permastead\current_user.txt");
            }
            else
            {
                //linux or macos
                fi = new FileInfo(userFolder + @"/.config/permastead/current_user.txt");
            }
            
            if (fi.Exists)
            {
                //write the user id to file location
                try
                {
                    File.WriteAllText(fi.FullName, userId.ToString().Trim());
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                File.WriteAllText(fi.FullName, userId.ToString().Trim());
            }

            return true;
        }
        
        public static List<T> LoadDataLocal<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sql, parameters).ToList();

                return rows;
            }
        }
    }
}
