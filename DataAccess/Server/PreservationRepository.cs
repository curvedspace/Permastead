using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class PreservationRepository
{
     public static List<FoodPreservation> GetAll(string conn)
    {
        try
        {
            var foodPreservations = new List<FoodPreservation>();
            FoodPreservation item;

            string sqlQuery = "select p.id, p.description, p.creationdate, p.startdate, p.enddate, p.rating, p.measurement, " +
            "p.measurementtypeid, m.code, m.description, p.comment, p.preservationtypeid,  " +
            "pt.description, p.authorid, p2.firstname, p2.lastname, p.preservationentityid, h.description  " +
            "from preservation p, preservationtype pt, measurementtype m, person p2, harvest h  " +
            "where p.measurementtypeid = m.id  " +
            "and p.preservationtypeid = pt.id  " +
            "and p.authorid = p2.id  " +
            "and p.preservationentityid = h.id  " +
            "union  " +
            "select p.id, p.description, p.creationdate, p.startdate, p.enddate, p.rating, p.measurement,  " +
            "p.measurementtypeid, m.code, m.description, p.comment, p.preservationtypeid,  " +
            "pt.description, p.authorid, p2.firstname, p2.lastname, p.preservationentityid, 'Unavailable'  " +
            "from preservation p, preservationtype pt, measurementtype m, person p2  " +
            "where p.measurementtypeid = m.id  " +
            "and p.preservationtypeid = pt.id  " +
            "and p.authorid = p2.id  " +
            "and p.preservationentityid is null ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        item = new FoodPreservation();
                        item.Id = Convert.ToInt64(dr[0].ToString());
                        item.Name = dr[1].ToString()!;
                        item.CreationDate = Convert.ToDateTime(dr[2].ToString());
                        item.StartDate = Convert.ToDateTime(dr[3].ToString());
                        
                        if (dr[4] != DBNull.Value)
                        {
                            item.EndDate = Convert.ToDateTime(dr[4].ToString());
                        }
                        
                        item.Rating = Convert.ToInt64(dr[5].ToString());
                        item.Measurement = Convert.ToInt64(dr[6].ToString());
                        item.Units.Id = Convert.ToInt64(dr[7].ToString());
                        item.Units.Code = dr[8].ToString()!;
                        item.Units.Description = dr[9].ToString()!;
                        item.Comment = dr[10].ToString()!;
                            
                        if (item.Type != null)
                        {
                            item.Type.Id = Convert.ToInt64(dr[11].ToString());
                            item.Type.Description = dr[12].ToString()!;
                        }
                        
                        item.Author = new Person();
                        item.Author.Id = Convert.ToInt64(dr[13].ToString());
                        item.Author.FirstName = dr[14].ToString();
                        item.Author.LastName = dr[15].ToString();

                        if (dr[16] != DBNull.Value)
                        {
                            item.PreservationEntity.Id = Convert.ToInt64(dr[16].ToString());
                        }
                        else
                        {
                            item.PreservationEntity.Id = 0;
                        }
                        
                        item.PreservationEntity.Name = dr[17].ToString();
                        
                        foodPreservations.Add(item);
                    }
                }
            }

            foodPreservations = foodPreservations.OrderByDescending(h => h.StartDate).ToList();
            return foodPreservations;
        }
        catch
        {
            return new List<FoodPreservation>();
        }
    }
    
    public static bool Insert(FoodPreservation item)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO Preservation (HarvestTypeId, HarvestEntityId, Description, Measurement, MeasurementTypeId, Comment, AuthorId, CreationDate, HarvestDate) " +
                                  "VALUES(@HarvestTypeId, @HarvestEntityId, @Description, @Measurement, @MeasurementTypeId, @Comment, @AuthorId, CURRENT_DATE, @HarvestDate);";
        
                return (db.Execute(sqlQuery, item) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(FoodPreservation item)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "UPDATE Preservation SET HarvestTypeId = @HarvestTypeId, HarvestEntityId = @HarvestEntityId, Description = @Description, HarvestDate = @HarvestDate, Measurement = @Measurement, " +
                                  "MeasurementTypeId = @MeasurementTypeId, Comment = @Comment, AuthorId = @AuthorId " + 
                                  "WHERE Id = @Id;";

                return (db.Execute(sqlQuery, item) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
}