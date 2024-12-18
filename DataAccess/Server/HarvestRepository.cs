using System.Data;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public class HarvestRepository
{
    public static List<Harvest> GetAll(string conn)
    {
        try
        {
            var harvests = new List<Harvest>();
            Harvest item;

            string sqlQuery =
                "select h.id, h.harvesttypeid, ht.description, h.auxid, pl.description as auxname, h.description, h.measurement, h.measurementtypeid, mt.code, mt.description, h.comment, h.creationdate, h.harvestdate, h.authorid, p.firstname, p.lastname " + 
                "from harvest h, harvesttype ht, measurementtype mt, person p, planting pl "+ 
                "where h.harvesttypeid = ht.id and h.auxid = pl.id and ht.description = 'Plant' and h.authorid = p.id and h.measurementtypeid = mt.id " + 
                "union select h.id, h.harvesttypeid, ht.description, h.auxid, a.name as auxname, h.description, h.measurement, h.measurementtypeid, mt.code, mt.description, h.comment, h.creationdate, h.harvestdate, h.authorid, p.firstname, p.lastname " + 
                "from harvest h, harvesttype ht, measurementtype mt, person p, animal a " + 
                "where h.harvesttypeid = ht.id and ht.description = 'Animal' and h.authorid = p.id and h.measurementtypeid = mt.id " + 
                "union select h.id, h.harvesttypeid, ht.description, h.auxid, 'Unknown' as auxname, h.description, h.measurement, h.measurementtypeid, mt.code, mt.description, h.comment, h.creationdate, h.harvestdate, h.authorid, p.firstname, p.lastname " + 
                "from harvest h, harvesttype ht, measurementtype mt, person p, animal a where h.harvesttypeid = ht.id and ht.description = 'Material' and h.authorid = p.id and h.measurementtypeid = mt.id " + 
                "union select h.id, h.harvesttypeid, ht.description, h.auxid, 'Unknown' as auxname, h.description, h.measurement, h.measurementtypeid, mt.code, mt.description, h.comment, h.creationdate, h.harvestdate, h.authorid, p.firstname, p.lastname " + 
                "from harvest h, harvesttype ht, measurementtype mt, person p, animal a where h.harvesttypeid = ht.id and ht.description = 'Other' and h.authorid = p.id and h.measurementtypeid = mt.id ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        item = new Harvest();
                        item.Id = Convert.ToInt64(dr[0].ToString());
                        
                        if (item.HarvestType != null)
                        {
                            item.HarvestType.Id = Convert.ToInt64(dr[1].ToString());
                            item.HarvestType.Description = dr[2].ToString()!;
                        }
                        
                        item.HarvestEntity.Id = Convert.ToInt64(dr[3].ToString());
                        item.HarvestEntity.Name = dr[4].ToString();
                        
                        item.Description = dr[5].ToString()!;
                        item.Measurement = Convert.ToInt64(dr[6].ToString());
                        item.Units.Id = Convert.ToInt64(dr[7].ToString());
                        item.Units.Code = dr[8].ToString()!;
                        item.Units.Description = dr[9].ToString()!;
                        
                        item.Comment = dr[10].ToString()!;
                        
                        item.CreationDate = Convert.ToDateTime(dr[11].ToString());
                        item.HarvestDate = Convert.ToDateTime(dr[12].ToString());

                        item.Author = new Person();
                        item.Author.Id = Convert.ToInt64(dr[13].ToString());
                        item.Author.FirstName = dr[14].ToString();
                        item.Author.LastName = dr[15].ToString();

                        harvests.Add(item);
                    }
                }
            }

            return harvests;
        }
        catch
        {
            return new List<Harvest>();
        }
    }
}