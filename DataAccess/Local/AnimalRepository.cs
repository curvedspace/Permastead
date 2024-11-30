using System.Data;
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
                      "a.StartDate, a.EndDate, a.comment " +
                      "FROM Animal a, Animalype at, Person p  " +
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
}