using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public static class AnimalRepository
{
    public static List<Animal> GetAll(string connectionString)
    {
        var myAnimals = new List<Animal>();
        Animal animal;

        try
        {
            var sql = "SELECT a.Id, a.Name, a.AnimalTypeId, at.Description AnimalTypeDesc, a.AuthorId, " +
                      "p.FirstName AuthorFirstName, p.LastName AuthorLastName, " +
                      "a.nickname, a.breed, a.birthday, " + 
                      "a.StartDate, a.EndDate, a.comment, a.ispet, a.tags " +
                      "FROM Animal a, AnimalType at, Person p  " +
                      "WHERE a.AnimalTypeId = at.Id " +
                      "AND a.AuthorId = p.Id " +
                      "ORDER BY a.StartDate DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        animal = new Animal();

                        animal.Id = Convert.ToInt64(dr[0].ToString());
                        animal.Name = dr[1].ToString();
                        animal.Type.Id = Convert.ToInt64(dr[2].ToString());
                        animal.Type.Description = dr[3].ToString();

                        animal.Author = new Person();
                        animal.Author.Id = Convert.ToInt64(dr[4].ToString());
                        animal.Author.FirstName = dr[5].ToString();
                        animal.Author.LastName = dr[6].ToString();
                        
                        animal.NickName = dr[7].ToString();
                        animal.Breed = dr[8].ToString();
                        
                        if (dr[9] != DBNull.Value)
                            animal.Birthday = Convert.ToDateTime(dr[9].ToString());
                        
                        if (dr[10] != DBNull.Value)
                            animal.StartDate = Convert.ToDateTime(dr[10].ToString());
                        
                        if (dr[11] != DBNull.Value)
                            animal.EndDate = Convert.ToDateTime(dr[11].ToString());
                        
                        animal.Comment = dr[12].ToString();
                        
                        animal.IsPet = Convert.ToBoolean(dr[13].ToString());
                        
                        var tagText = dr[14].ToString()!.Trim();

                        if (tagText != null && tagText.Length > 0)
                        {
                            animal.Tags = tagText.Trim();
                            animal.TagList = tagText.Split(' ').ToList();
                        }

                        myAnimals.Add(animal);
                    }
                }
            }

            return myAnimals;

        }
        catch (Exception)
        {
            return myAnimals;
        }
    }

    public static bool Insert(Animal item)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO Animal (AnimalTypeId, Nickname, Name, Breed, AuthorId, Comment, Birthday, StartDate, EndDate, IsPet, Tags) " +
                                  "VALUES(@AnimalTypeId, @Nickname, @Name, @Breed, @AuthorId, @Comment, @Birthday, @StartDate, @EndDate, @IsPet, @Tags);";
        
                return (db.Execute(sqlQuery, item) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(Animal item)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "UPDATE Animal SET AnimalTypeId = @AnimalTypeId, Nickname = @Nickname, Name = @Name, Breed = @Breed, Birthday = @Birthday, " +
                                  "StartDate = @StartDate, Comment = @Comment, AuthorId = @AuthorId, EndDate = @EndDate, IsPet = @IsPet, Tags = @Tags " + 
                                  "WHERE Id = @Id;";

                return (db.Execute(sqlQuery, item) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool InsertAnimalObservation(string connectionString, AnimalObservation obs)
    {
        var rtnValue = false;

        var sql = "INSERT INTO AnimalObservation (AnimalId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                  "VALUES(:animalId, :comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', :commentTypeId, :authorId) ";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":animalId", obs.AnimalId);
                command.Parameters.AddWithValue(":comment", obs.Comment);
                command.Parameters.AddWithValue(":commentTypeId", obs.CommentType!.Id);
                command.Parameters.AddWithValue(":authorId", obs.Author!.Id);

                rtnValue = (command.ExecuteNonQuery() == 1);
            }
        }

        return rtnValue;
    }
    
    public static bool UpdateAnimalObservation(string connectionString, Observation obs)
    {
        var rtnValue = false;

        var sql = "UPDATE AnimalObservation SET Comment = @Comment  " +
                  "WHERE  id = @Id; ";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":id", obs.Id);
                command.Parameters.AddWithValue(":comment", obs.Comment);

                rtnValue = (command.ExecuteNonQuery() == 1);
            }
        }

        return rtnValue;
    }

    public static List<AnimalObservation> GetAllAnimalObservations(string connectionString)
    {
        {
            var myObs = new List<AnimalObservation>();
            AnimalObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.AnimalId, a.Name " +
                      "FROM AnimalObservation o, CommentType ct, Person p, Animal a " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.AnimalId = a.Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new AnimalObservation();
                        
                        o.Comment = dr[0].ToString()!;
                        o.CreationDate = Convert.ToDateTime(dr[1].ToString());
                        o.StartDate = Convert.ToDateTime(dr[2].ToString());
                        o.EndDate = Convert.ToDateTime(dr[3].ToString());

                        o.CommentType = new CommentType();
                        o.CommentType.Id = Convert.ToInt64(dr[4].ToString());
                        o.CommentType.Description = dr[5].ToString();
                        // o.CommentType.Id = -2;
                        // o.CommentType.Description = "Animal";

                        o.Author = new Person();
                        o.Author.Id = Convert.ToInt64(dr[6].ToString());
                        o.Author.FirstName = dr[7].ToString();
                        o.Author.LastName = dr[8].ToString();

                        o.Id = Convert.ToInt64(dr[9].ToString());
                        o.Animal.Id = Convert.ToInt64(dr[10].ToString());
                        o.Animal.Name = dr[11].ToString();
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
      
    public static List<AnimalObservation> GetAllObservationsForAnimal(string connectionString, long animalId)
    {
        {
            var myObs = new List<AnimalObservation>();
            AnimalObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.AnimalId, a.Name " +
                      "FROM AnimalObservation o, CommentType ct, Person p, Animal a " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.AnimalId = a.Id " +
                      "AND o.AnimalId = @Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", animalId);
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new AnimalObservation();
                        
                        o.Comment = dr[0].ToString()!;
                        o.CreationDate = Convert.ToDateTime(dr[1].ToString());
                        o.StartDate = Convert.ToDateTime(dr[2].ToString());
                        o.EndDate = Convert.ToDateTime(dr[3].ToString());

                        o.CommentType = new CommentType();
                        o.CommentType.Id = Convert.ToInt64(dr[4].ToString());
                        o.CommentType.Description = dr[5].ToString();

                        o.Author = new Person();
                        o.Author.Id = Convert.ToInt64(dr[6].ToString());
                        o.Author.FirstName = dr[7].ToString();
                        o.Author.LastName = dr[8].ToString();

                        o.Id = Convert.ToInt64(dr[9].ToString());
                        o.Animal.Id = Convert.ToInt64(dr[10].ToString());
                        o.Animal.Name = dr[11].ToString();
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
    
    public static Observation GetObservationById(string connectionString, long id)
    {
        Observation o = null;

        var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                  "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id " +
                  "FROM AnimalObservation o, CommentType ct, Person p " +
                  "WHERE ct.Id = o.CommentTypeId " +
                  "AND o.Id = @id " +
                  "AND p.Id = o.AuthorId ";

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
                    o = new Observation();

                    o.Comment = dr[0].ToString()!;
                    o.CreationDate = Convert.ToDateTime(dr[1].ToString());
                    o.StartDate = Convert.ToDateTime(dr[2].ToString());
                    o.EndDate = Convert.ToDateTime(dr[3].ToString());

                    o.CommentType = new CommentType();
                    o.CommentType.Id = Convert.ToInt64(dr[4].ToString());
                    o.CommentType.Description = dr[5].ToString();

                    o.Author = new Person();
                    o.Author.Id = Convert.ToInt64(dr[6].ToString());
                    o.Author.FirstName = dr[7].ToString();
                    o.Author.LastName = dr[8].ToString();

                    o.Id = Convert.ToInt64(dr[9].ToString());
                    o.AsOfDate = o.CreationDate;
                    
                }
            }
        }
        
        return o;
    }
    
}