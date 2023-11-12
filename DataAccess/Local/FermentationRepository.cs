using Dapper;
using Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace DataAccess.Local;

public class FermentationRepository
{
    public static List<Fermentation> GetAll(string conn)
    {
        try
        {
            var myItems = new List<Fermentation>();
            Fermentation fermentation;

            string sqlQuery =
                "SELECT f.Id, f.Code, f.Description, r.Id, r.Code as RecipeCode, r.Description as RecipeDescription, " +
                "f.Rating, f.Amount, f.CreationDate, f.StartDate, f.EndDate, f.AuthorId, p.FirstName, p.LastName  " +
                "FROM Fermentation f, Recipe r, Person p " +
                "WHERE f.RecipeId  = r.Id  " +
                "AND f.AuthorId = p.Id ";

            using (IDbConnection connection = new SqliteConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        fermentation = new Fermentation();
                        fermentation.Id = Convert.ToInt64(dr[0].ToString());
                        fermentation.Code = dr[1].ToString()!;
                        fermentation.Description = dr[2].ToString()!;
                        fermentation.Recipe.Id = Convert.ToInt64(dr[3].ToString());
                        
                        fermentation.Recipe.Code = dr[4].ToString()!;
                        fermentation.Recipe.Description = dr[5].ToString()!;
                        fermentation.Rating = Convert.ToInt64(dr[6].ToString());
                        fermentation.Amount = Convert.ToInt64(dr[7].ToString());
                        
                        fermentation.CreationDate = Convert.ToDateTime(dr[8].ToString());
                        fermentation.StartDate = Convert.ToDateTime(dr[9].ToString());
                        fermentation.EndDate = Convert.ToDateTime(dr[10].ToString());
                        
                        fermentation.Author = new Person();
                        fermentation.Author.Id = Convert.ToInt64(dr[11].ToString());
                        fermentation.Author.FirstName = dr[12].ToString();
                        fermentation.Author.LastName = dr[13].ToString();
                        
                        myItems.Add(fermentation);
                    }
                }
            }

            return myItems;
        }
        catch
        {
            return new List<Fermentation>();
        }
    }
    
}