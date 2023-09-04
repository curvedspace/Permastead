using Dapper;

using Models;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Local
{
    public class GardenBedTypeRepository
    {
        public static List<GardenBedType> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM GardenBedType ORDER BY Description;";

                    return db.Query<GardenBedType>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<GardenBedType>();
            }
        }
        
        public static bool Insert(GardenBedType gbt)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO GardenBedType (Code, Description, CreationDate, StartDate, EndDate, AuthorId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";
        
                    return (db.Execute(sqlQuery, gbt) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
