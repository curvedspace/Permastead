using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public class AnimalRepository
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
                      "a.StartDate, a.EndDate, a.comment, a.ispet " +
                      "FROM Animal a, AnimalType at, Person p  " +
                      "WHERE a.AnimalTypeId = at.Id " +
                      "AND a.AuthorId = p.Id " +
                      "ORDER BY a.StartDate DESC";

            using (IDbConnection connection = new SqliteConnection(connectionString))
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
                        
                        animal.Birthday = Convert.ToDateTime(dr[9].ToString());
                        animal.StartDate = Convert.ToDateTime(dr[10].ToString());
                        animal.EndDate = Convert.ToDateTime(dr[11].ToString());
                        
                        animal.Comment = dr[12].ToString();
                        animal.IsPet = Convert.ToBoolean(Convert.ToInt32(dr[13].ToString()));

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
    
    public static bool InsertAnimalObservation(string connectionString, AnimalObservation obs)
    {
        var rtnValue = false;

        var sql = "INSERT INTO AnimalObservation (AnimalId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                  "VALUES($animalId, $comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', $commentTypeId, $authorId) ";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("$animalId", obs.AnimalId);
                command.Parameters.AddWithValue("$comment", obs.Comment);
                command.Parameters.AddWithValue("$commentTypeId", obs.CommentType!.Id);
                command.Parameters.AddWithValue("$authorId", obs.Author!.Id);

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

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
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

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
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
    
    public static bool Insert(Animal animal)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO Animal (AnimalTypeId, NickName, Name, Breed, AuthorId, Comment, Birthday, StartDate, EndDate, IsPet) " +
                                                    "VALUES(@AnimalTypeId, @NickName, @Name, @Breed, @AuthorId, @Comment, @Birthday, @StartDate, @EndDate, @IsPet);";

                return (db.Execute(sqlQuery, animal) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(Animal animal)
    {
        try
        {
            
            if (animal != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "UPDATE Animal SET AnimalTypeId = @AnimalTypeId, Nickname = @NickName, Name = @Name, Breed = @Breed, Birthday = @Birthday, " +
                                      "StartDate = @StartDate, Comment = @Comment, AuthorId = @AuthorId, EndDate = @EndDate, IsPet = @IsPet " + 
                                      "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, animal) == 1);
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