
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;

using Models;

namespace DataAccess.Local
{
    public class VendorRepository
    {
        public static List<Vendor> GetAll(string conn)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(conn))
                {
                    string sqlQuery = "SELECT * FROM Vendor ORDER BY Description;";

                    return db.Query<Vendor>(sqlQuery).ToList();
                }
            }
            catch
            {
                return new List<Vendor>();
            }
        }


        public static bool Insert(Vendor vendor)
        {
            try
            {
                using IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource());
                string sqlQuery = "INSERT INTO Vendor (Code, Description, CreationDate, StartDate, EndDate, Rating) " +
                                  "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @Rating);";

                return (db.Execute(sqlQuery, vendor) == 1);
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Update(Vendor vendor)
        {
            try
            {
                
                if (vendor != null)
                {
                    using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                    {
                        string sqlQuery =
                            "UPDATE Vendor SET Code = @Code, Description = @Description, Rating = @Rating, StartDate = @StartDate, EndDate = @EndDate " +
                            "WHERE Id = @Id;";

                        return (db.Execute(sqlQuery, vendor) == 1);
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
