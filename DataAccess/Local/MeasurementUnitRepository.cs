
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
    }
}
