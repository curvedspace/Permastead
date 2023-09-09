
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DataAccess.Local;

public class SettingsRepository
{

    public static Dictionary<string, string> GetAll(string conn)
    {
        var rtnDict = new Dictionary<string, string>();
        
        try
        {
            string sqlQuery = "SELECT Code, Description FROM Settings;";
            
            using (IDbConnection connection = new SqliteConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var code = dr[0].ToString();
                        var description = dr[1].ToString();

                        if (!string.IsNullOrEmpty(code))
                        {
                            rtnDict.Add(code, description!.ToString());
                        }
                        
                    }
                }
            }

            return rtnDict;
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    public static bool Insert(string code, string description)
    {
        try
        {
            string sqlQuery = "INSERT INTO Settings (Code, Description, CreationDate, LastUpdated) " +
                              "VALUES(@Code, @Description, CURRENT_DATE,CURRENT_DATE);";

            using (SqliteConnection connection = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.Parameters.AddWithValue("@Code",code);
                    command.Parameters.AddWithValue("@Description",description);
                    
                    return (command.ExecuteNonQuery() == 1);

                }
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(string code, string description)
    {
        try
        {
            
            if (code != null && description != null)
            {
                using (SqliteConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE Settings SET Description = @Description, LastUpdated = CURRENT_DATE " +
                        "WHERE Code = @Code;";
                    
                    db.Open();
                    using (SqliteCommand command = db.CreateCommand())
                    {
                        command.CommandText = sqlQuery;
                        command.Parameters.AddWithValue("@Code",code);
                        command.Parameters.AddWithValue("@Description",description);
            
                        return (command.ExecuteNonQuery() == 1);
                    }
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
