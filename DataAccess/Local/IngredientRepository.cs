using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Dapper;

using Microsoft.Data.Sqlite;

using Models;


namespace DataAccess.Local
{
    public static class IngredientRepository
    {
        public static bool Insert(Ingredient ingredient)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Ingredient (Code, Description, CreationDate, StartDate, EndDate, PreferredVendorId, Notes) " +
                                      "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @PreferredVendorId, @Notes);";

                    return (db.Execute(sqlQuery, ingredient) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
