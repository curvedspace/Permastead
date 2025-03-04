using System.Data;
using System.Diagnostics.Metrics;
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
            "pt.description, p.authorid, p2.firstname, p2.lastname, p.harvestid, h.description  " +
            "from preservation p, preservationtype pt, measurementtype m, person p2, harvest h  " +
            "where p.measurementtypeid = m.id  " +
            "and p.preservationtypeid = pt.id  " +
            "and p.authorid = p2.id  " +
            "and p.harvestid = h.id  " +
            "union  " +
            "select p.id, p.description, p.creationdate, p.startdate, p.enddate, p.rating, p.measurement,  " +
            "p.measurementtypeid, m.code, m.description, p.comment, p.preservationtypeid,  " +
            "pt.description, p.authorid, p2.firstname, p2.lastname, p.harvestid, 'Unavailable'  " +
            "from preservation p, preservationtype pt, measurementtype m, person p2  " +
            "where p.measurementtypeid = m.id  " +
            "and p.preservationtypeid = pt.id  " +
            "and p.authorid = p2.id  " +
            "and (p.harvestid is null or p.harvestid = 0)";

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
                            
                        if (item.PreservationType != null)
                        {
                            item.PreservationType.Id = Convert.ToInt64(dr[11].ToString());
                            item.PreservationType.Description = dr[12].ToString()!;
                        }
                        
                        item.Author = new Person();
                        item.Author.Id = Convert.ToInt64(dr[13].ToString());
                        item.Author.FirstName = dr[14].ToString();
                        item.Author.LastName = dr[15].ToString();

                        if (dr[16] != DBNull.Value)
                        {
                            item.Harvest.Id = Convert.ToInt64(dr[16].ToString());
                        }
                        else
                        {
                            item.Harvest.Id = 0;
                        }
                        
                        item.Harvest.Description = dr[17].ToString();
                        
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
    
    public static bool Insert(string connectionString, FoodPreservation item)
    {
        try
        {
            bool rtnValue = false;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO Preservation (PreservationTypeId, HarvestId, Description, Measurement, Rating, MeasurementTypeId, Comment, AuthorId, CreationDate, StartDate) " +
                                  "VALUES(:preservationTypeId, :harvestId, :description, :measurement, :rating, :measurementTypeId, :comment, :authorId, CURRENT_DATE, :startDate);";
        
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    
                    command.Parameters.AddWithValue(":description", item.Name);
                    command.Parameters.AddWithValue(":startDate", item.StartDate);
                    command.Parameters.AddWithValue(":measurement", item.Measurement);
                    command.Parameters.AddWithValue(":rating", item.Rating);
                    command.Parameters.AddWithValue(":measurementTypeId", item.MeasurementTypeId);
                    command.Parameters.AddWithValue(":comment", item.Comment);
                    command.Parameters.AddWithValue(":authorId", item.Author!.Id);
                    command.Parameters.AddWithValue(":harvestId", item.HarvestId);
                    command.Parameters.AddWithValue(":preservationTypeId", item.PreservationTypeId);
                    
                    rtnValue = (command.ExecuteNonQuery() == 1);
                }

            }
            
            return rtnValue;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(string connectionString, FoodPreservation item)
    {
        try
        {
            bool rtnValue = false;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Preservation SET Description = :description, StartDate = :startDate, Measurement = :measurement, Rating = :rating, " +
                                  "MeasurementTypeId = :measurementTypeId, Comment = :comment, AuthorId = :authorId, HarvestId = :harvestId, PreservationTypeId = :preservationTypeId " + 
                                  "WHERE Id = :id;";

                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    
                    command.Parameters.AddWithValue(":description", item.Name);
                    command.Parameters.AddWithValue(":startDate", item.StartDate);
                    command.Parameters.AddWithValue(":measurement", item.Measurement);
                    command.Parameters.AddWithValue(":rating", item.Rating);
                    command.Parameters.AddWithValue(":measurementTypeId", item.MeasurementTypeId);
                    command.Parameters.AddWithValue(":comment", item.Comment);
                    command.Parameters.AddWithValue(":authorId", item.Author!.Id);
                    command.Parameters.AddWithValue(":harvestId", item.HarvestId);
                    command.Parameters.AddWithValue(":preservationTypeId", item.PreservationTypeId);
                    command.Parameters.AddWithValue(":id", item.Id);
                    
                    rtnValue = (command.ExecuteNonQuery() == 1);
                }
                
            }
            
            return rtnValue;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool InsertPreservationObservation(string connectionString, FoodPreservationObservation obs)
    {
        var rtnValue = false;

        var sql = "INSERT INTO PreservationObservation (PreservationId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                  "VALUES(:preservationId, :comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', :commentTypeId, :authorId) ";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":preservationId", obs.FoodPreservationId);
                command.Parameters.AddWithValue(":comment", obs.Comment);
                command.Parameters.AddWithValue(":commentTypeId", obs.CommentType!.Id);
                command.Parameters.AddWithValue(":authorId", obs.Author!.Id);

                rtnValue = (command.ExecuteNonQuery() == 1);
            }
        }

        return rtnValue;
    }

    public static List<FoodPreservationObservation> GetAllPreservationObservations(string connectionString)
    {
        {
            var myObs = new List<FoodPreservationObservation>();
            FoodPreservationObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.PreservationId, a.Name " +
                      "FROM PreservationObservation o, CommentType ct, Person p, Preservation a " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.PreservationId = a.Id " +
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
                        o = new FoodPreservationObservation();
                        
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
                        o.FoodPreservation.Id = Convert.ToInt64(dr[10].ToString());
                        o.FoodPreservation.Name = dr[11].ToString()!;
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
      
    public static List<FoodPreservationObservation> GetAllObservationsForPreservation(string connectionString, long preservationId)
    {
        {
            var myObs = new List<FoodPreservationObservation>();
            FoodPreservationObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.PreservationId, a.Description " +
                      "FROM PreservationObservation o, CommentType ct, Person p, Preservation a " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.PreservationId = a.Id " +
                      "AND o.PreservationId = @Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", preservationId);
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new FoodPreservationObservation();
                        
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
                        o.FoodPreservation.Id = Convert.ToInt64(dr[10].ToString());
                        o.FoodPreservation.Name = dr[11].ToString()!;
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
}