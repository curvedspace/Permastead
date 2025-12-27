using Dapper;
using System.Data;
using Npgsql;

using Models;

namespace DataAccess.Server
{
    public static class GardenBedTypeRepository
    {
        public static List<GardenBedType> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(conn))
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
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
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
