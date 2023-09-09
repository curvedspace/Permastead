using System.Data;
using System.Runtime.InteropServices;

using Dapper;

using Microsoft.Data.Sqlite;


namespace DataAccess
{
    public static class DataConnection 
    {
        public static string GetLocalDataSource()
        {
            return @"data source=" + GetDefaultDatabaseLocation() + ";";
        }

        public static string GetDefaultDatabaseLocation()
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string dbLocation = "";
            
            //try to read in a db_location.txt file
            var dbLoc = new FileInfo(userFolder + @"\.config\permastead\db_location.txt");
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
