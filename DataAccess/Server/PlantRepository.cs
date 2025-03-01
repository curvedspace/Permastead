using Dapper;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Models;
using Npgsql;

namespace DataAccess.Server
{
    public static class PlantRepository
    {
        public static List<Plant> GetPlants(string connectionString)
        {
            var myPlants = new List<Plant>();
            Plant plant;

            var sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                "per.FirstName, per.LastName, p.Id, p.code " +
                "FROM Plant p, Person per " + 
                "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) ORDER BY p.Description ASC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        plant = new Plant();

                        plant.Description = dr[0].ToString()!;
                        plant.Comment = dr[1].ToString()!;
                        plant.Family = dr[2].ToString()!;
                        plant.Url = dr[3].ToString()!;

                        plant.CreationDate = Convert.ToDateTime(dr[4].ToString());
                        plant.StartDate = Convert.ToDateTime(dr[5].ToString());
                        plant.EndDate = Convert.ToDateTime(dr[6].ToString());

                        plant.Author = new Person();
                        plant.Author.Id = Convert.ToInt64(dr[7].ToString());
                        plant.Author.FirstName = dr[8].ToString();
                        plant.Author.LastName = dr[9].ToString();

                        plant.Id = Convert.ToInt64(dr[10].ToString());
                        plant.Code = dr[11].ToString()!;

                        myPlants.Add(plant);
                    }
                }

                return myPlants;
            }
        }

        public static Plant GetPlantFromId(string connectionString, long id)
        {
            Plant plant = new Plant();

            var sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                "per.FirstName, per.LastName, p.Id, p.code " +
                "FROM Plant p, Person per " + 
                "WHERE per.Id = p.AuthorId " +
                "AND p.Id = @id ";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new NpgsqlParameter("@id", id));
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        plant = new Plant();

                        plant.Description = dr[0].ToString()!;
                        plant.Comment = dr[1].ToString()!;
                        plant.Family = dr[2].ToString()!;
                        plant.Url = dr[3].ToString()!;

                        plant.CreationDate = Convert.ToDateTime(dr[4].ToString());
                        plant.StartDate = Convert.ToDateTime(dr[5].ToString());
                        plant.EndDate = Convert.ToDateTime(dr[6].ToString());

                        plant.Author = new Person();
                        plant.Author.Id = Convert.ToInt64(dr[7].ToString());
                        plant.Author.FirstName = dr[8].ToString();
                        plant.Author.LastName = dr[9].ToString();

                        plant.Id = Convert.ToInt64(dr[10].ToString());
                        plant.Code = dr[11].ToString()!;

                    }
                }
            }

            return plant;
            
        }

        public static bool Insert(Plant plant)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "INSERT INTO Plant (Code, Description, StartDate, EndDate, CreationDate, Comment, Family, Url, AuthorId) " +
                        "VALUES(@Code, @Description, @StartDate, @EndDate, CURRENT_DATE, @Comment, @Family, @Url, @AuthorId);";

                    return (db.Execute(sqlQuery, plant) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Update(Plant plant)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "UPDATE Plant SET Code = @Code, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, " +
                                      "Comment = @Comment, Family = @Family, Url = @Url, AuthorId = @AuthorId " + 
                                      "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, plant) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Delete(Plant plant)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "UPDATE Plant SET EndDate = @EndDate WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, plant) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
