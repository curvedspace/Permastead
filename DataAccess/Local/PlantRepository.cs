using Dapper;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Models;

namespace DataAccess.Local
{
    public static class PlantRepository
    {
        public static List<Plant> GetPlants(string connectionString)
        {
            var myPlants = new List<Plant>();
            Plant plant;

            var sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                "per.FirstName, per.LastName, p.Id, p.code, p.Tags " +
                "FROM Plant p, Person per " + 
                "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > DATE()) ORDER BY p.Description ASC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
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

                        var tagText = dr[12].ToString()!.Trim();

                        if (tagText != null && tagText.Length > 0)
                        {
                            plant.Tags = tagText.Trim();
                            plant.TagList = tagText.Split(' ').ToList();
                        }
                        
                        myPlants.Add(plant);
                    }
                }

                return myPlants;
            }
        }
        
        public static List<Plant> GetPlantsWithStarters(string connectionString)
        {
            var myPlants = new List<Plant>();
            Plant plant;

            var sql = "SELECT DISTINCT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                "per.FirstName, per.LastName, p.Id, p.code, p.Tags " +
                "FROM Plant p, Person per, Seedpacket s  " + 
                "WHERE per.Id = p.AuthorId " +
                "AND s.plantid = p.id AND s.enddate > DATE() " +
                "AND (p.EndDate is null OR p.EndDate > DATE()) ORDER BY p.Description ASC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
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

                        var tagText = dr[12].ToString()!.Trim();

                        if (tagText != null && tagText.Length > 0)
                        {
                            plant.Tags = tagText.Trim();
                            plant.TagList = tagText.Split(' ').ToList();
                        }
                        
                        myPlants.Add(plant);
                    }
                }

                return myPlants;
            }
        }

        public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
        {
            var results = new List<SearchResult>();
            
            var sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                      "per.FirstName, per.LastName, p.Id, p.code " +
                      "FROM Plant p, Person per " + 
                      "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) " + 
                      "AND lower(p.Comment) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                      "ORDER BY p.CreationDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[4].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[10].ToString());
                        result.Entity.Name = "Plant";
                        result.SubType = dr[0].ToString()!;
                        result.FieldName = "Comment";
                        result.SearchText = dr[1].ToString()!;

                        results.Add(result);
                    }
                }
            }
            sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                  "per.FirstName, per.LastName, p.Id, p.code, p.tags " +
                  "FROM Plant p, Person per " + 
                  "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) " + 
                  "AND lower(p.Tags) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY p.CreationDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[4].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[10].ToString());
                        result.Entity.Name = "Plant";
                        result.SubType = dr[0].ToString()!;
                        result.FieldName = "Tags";
                        result.SearchText = dr[12].ToString()!;

                        results.Add(result);
                    }
                }
            }

            sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                  "per.FirstName, per.LastName, p.Id, p.code " +
                  "FROM Plant p, Person per " + 
                  "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) " + 
                  "AND lower(p.Description) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY p.CreationDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[4].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[10].ToString());
                        result.Entity.Name = "Plant";
                        result.SubType = dr[0].ToString()!;
                        result.FieldName = "Description";
                        result.SearchText = dr[0].ToString()!;

                        results.Add(result);
                    }
                }
            }

            sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                  "per.FirstName, per.LastName, p.Id, p.code " +
                  "FROM Plant p, Person per " + 
                  "WHERE per.Id = p.AuthorId AND (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) " + 
                  "AND lower(p.Family) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY p.CreationDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[4].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[10].ToString());
                        result.Entity.Name = "Plant";
                        result.SubType = dr[0].ToString()!;
                        result.FieldName = "Family";
                        result.SearchText = dr[2].ToString()!;

                        results.Add(result);
                    }
                }

            }
            
            return results;
            
        }
        
        public static Plant GetPlantFromId(string connectionString, long id)
        {
            Plant plant = new Plant();

            var sql = "SELECT p.Description, p.Comment, p.Family, p.Url, p.CreationDate, p.StartDate, p.EndDate, p.AuthorId, " + 
                "per.FirstName, per.LastName, p.Id, p.code, p.tags " +
                "FROM Plant p, Person per " + 
                "WHERE per.Id = p.AuthorId " +
                "AND p.Id = @id ";

            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    
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
                        
                        var tagText = dr[12].ToString()!.Trim();

                        if (tagText != null && tagText.Length > 0)
                        {
                            plant.Tags = tagText.Trim();
                            plant.TagList = tagText.Split(' ').ToList();
                        }

                    }
                }
            }

            return plant;
            
        }

        public static List<TagData> GetAllTags(string connectionString)
        {
            var tags = new List<TagData>();
            var sql = "SELECT p.tags " +
                      "FROM Plant p  " + 
                      "WHERE (p.EndDate is null OR p.EndDate > CURRENT_DATE+1) ";

            var stringTags = new List<string>();
            
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    
                    while (dr.Read())
                    {
                        if (dr[0].ToString()! != "")
                        {
                            var currentTags = dr[0].ToString()!.Trim().Split(' ').ToList();
                            foreach (var tagData in currentTags)
                            {
                                if (!stringTags.Contains(tagData))
                                {
                                    stringTags.Add(tagData);
                                }
                            }
                        }
                    }
                }

                foreach (var stringTag in stringTags)
                {
                    var td = new TagData
                    {
                        TagText = stringTag
                    };
                    tags.Add(td);
                }
            }

            return tags;
        }
        
        public static bool Insert(Plant plant)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO Plant (Code, Description, StartDate, EndDate, CreationDate, Comment, Family, Url, Tags, AuthorId) " +
                        "VALUES(@Code, @Description, @StartDate, @EndDate, CURRENT_DATE, @Comment, @Family, @Url, @Tags, @AuthorId);";

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
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "UPDATE Plant SET Code = @Code, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, " +
                                      "Comment = @Comment, Family = @Family, Url = @Url, AuthorId = @AuthorId , Tags = @Tags " + 
                                      "WHERE Id = @Id;";

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
