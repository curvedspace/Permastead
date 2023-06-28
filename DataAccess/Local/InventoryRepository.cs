using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Models;

namespace DataAccess.Local;

public static class InventoryRepository
{
    public static List<Inventory> GetAll(string conn)
    {
        try
        {
            var myInventory = new List<Inventory>();
            Inventory inv;

            string sqlQuery =
                "SELECT i.Id, i.Description, i.InventoryGroupId, i.InventoryTypeId, i.OriginalValue, i.CurrentValue, " +
                "i.Brand, i.Notes, i.CreationDate, i.StartDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName,  " +
                "it.Description it_desc, ig.Description ig_desc, i.Room, i.Quantity, i.ForSale " +
                "FROM Inventory i, InventoryGroup ig, InventoryType it, Person p " +
                "WHERE i.InventoryGroupId  = ig.Id  " +
                "AND i.InventoryTypeId  = it.Id  " +
                "AND i.AuthorId = p.Id ";

            using (IDbConnection connection = new SqliteConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        inv = new Inventory();
                        inv.Id = Convert.ToInt64(dr[0].ToString());
                        inv.Description = dr[1].ToString()!;
                        inv.InventoryGroup.Id = Convert.ToInt64(dr[2].ToString());
                        inv.InventoryType.Id = Convert.ToInt64(dr[3].ToString());
                        inv.OriginalValue = Convert.ToDouble(dr[4].ToString());
                        inv.CurrentValue = Convert.ToDouble(dr[5].ToString());
                        inv.Brand = dr[6].ToString()!;
                        inv.Notes = dr[7].ToString()!;
                        
                        inv.CreationDate = Convert.ToDateTime(dr[8].ToString());
                        inv.StartDate = Convert.ToDateTime(dr[9].ToString());
                        inv.EndDate = Convert.ToDateTime(dr[10].ToString());
                        inv.LastUpdated = Convert.ToDateTime(dr[11].ToString());

                        inv.Author = new Person();
                        inv.Author.Id = Convert.ToInt64(dr[12].ToString());
                        inv.Author.FirstName = dr[13].ToString();
                        inv.Author.LastName = dr[14].ToString();
                        
                        inv.InventoryType.Description = dr[15].ToString();
                        inv.InventoryGroup.Description = dr[16].ToString();

                        inv.Room = dr[17].ToString()!;
                        inv.Quantity = Convert.ToInt64(dr[18].ToString());
                        
                        inv.ForSale = dr[19].ToString() == "1";

                        myInventory.Add(inv);
                    }
                }
            }

            return myInventory;
        }
        catch
        {
            return new List<Inventory>();
        }
    }
    
    public static bool Insert(Inventory inventory)
    {
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO Inventory (Description, StartDate, EndDate, CreationDate, InventoryTypeId, InventoryGroupId, AuthorId, OriginalValue," +
                    "CurrentValue, Brand, Notes, Room, Quantity, ForSale, LastUpdated) " +
                    "VALUES(@Description, @StartDate, @EndDate, CURRENT_DATE, @InventoryTypeId, @InventoryGroupId, @AuthorId, @OriginalValue, @CurrentValue, " +
                    "@Brand, @Notes, @Room, @Quantity, @ForSale, CURRENT_TIMESTAMP);";

                return (db.Execute(sqlQuery, inventory) == 1);
            }
        }
        catch
        {
            return false;
        }
    }
    
    public static bool Update(Inventory inventory)
    {
        try
        {
            if (inventory != null)
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery =
                        "UPDATE Inventory SET Description = @Description, StartDate = @StartDate, EndDate = @EndDate, InventoryTypeId = @InventoryTypeId, " +
                        "InventoryGroupId = @InventoryGroupId, AuthorId = @AuthorId, OriginalValue = @OriginalValue, CurrentValue = @CurrentValue, " +
                        "Brand = @Brand, Notes = @Notes, Room = @Room, Quantity = @Quantity, ForSale = @ForSale, LastUpdated = CURRENT_TIMESTAMP " +
                        "WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, inventory) == 1);
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