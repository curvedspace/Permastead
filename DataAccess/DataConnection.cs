using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            if (isWindows)
            {
                return userFolder + @"\.config\permastead\permastead.db";
            }
            else
            {
                //linux or macos
                return userFolder + @"/.config/permastead/permastead.db";
            }
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
