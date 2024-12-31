
using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;
using Models;

namespace DataAccess.Local
{
    public class MeasurementUnitRepository
    {
        public static List<MeasurementUnit> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM MeasurementType ORDER BY Description;";

                    return db.Query<MeasurementUnit>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<MeasurementUnit>();
            }
        }
        
        public static bool Insert(MeasurementUnit measurementUnit)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO MeasurementType (Code, Description, CreationDate, StartDate, EndDate, AuthorId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";
        
                    return (db.Execute(sqlQuery, measurementUnit) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Update(MeasurementUnit type)
        {
            try
            {
                
                if (type != null)
                {
                    using (IDbConnection db = new SqliteConnection(DataConnection.GetServerConnectionString()))
                    {
                        string sqlQuery =
                            "UPDATE MeasurementType SET Description = @Description, AuthorId = @AuthorId, StartDate = @StartDate, EndDate = @EndDate " +
                            "WHERE Id = @Id;";

                        return (db.Execute(sqlQuery, type) == 1);
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
}
