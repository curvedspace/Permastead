using System.Data;
using Common;
using Dapper;
using Models;
using Npgsql;

namespace DataAccess.Server;

public static class InventoryRepository
{
    public static List<Inventory> GetAll(string conn)
    {
        try
        {
            var myInventory = new List<Inventory>();
            Inventory inv;

            string sqlQuery =
                "SELECT i.Id, i.Description, i.IGroup, i.IType, i.OriginalValue, i.CurrentValue, " +
                "i.Brand, i.Notes, i.CreationDate, i.StartDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName,  " +
                "i.Room, i.Quantity, i.ForSale " +
                "FROM Inventory i, Person p " +
                "WHERE i.AuthorId = p.Id ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
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
                        inv.InventoryGroup = dr[2].ToString()!;
                        inv.InventoryType = dr[3].ToString()!;
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
                        
                        inv.Room = dr[15].ToString()!;
                        inv.Quantity = Convert.ToInt64(dr[16].ToString());
                        
                        inv.ForSale = Convert.ToBoolean(dr[17].ToString());

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
    
        public static List<SearchResult> GetSearchResults(string connectionString, string searchText)
        {
            var results = new List<SearchResult>();
            
            var sql = "SELECT i.Id, i.Description, i.IGroup, i.IType, i.OriginalValue, i.CurrentValue, " +
                "i.Brand, i.Notes, i.CreationDate, i.StartDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName,  " +
                "i.Room, i.Quantity, i.ForSale " +
                "FROM Inventory i, Person p " +
                "WHERE i.AuthorId = p.Id " +
                "AND lower(i.Description) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                "ORDER BY i.CreationDate DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[8].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                        result.Entity.Name = "Inventory";
                        result.SubType = dr[1].ToString()!;
                        result.FieldName = "Description";
                        result.SearchText = TextUtils.GetSubstring(dr[1].ToString()!,0,DataConnection.SearchTextLength, true);

                        results.Add(result);
                    }
                }
            }

            sql = "SELECT i.Id, i.Description, i.IGroup, i.IType, i.OriginalValue, i.CurrentValue, " +
                "i.Brand, i.Notes, i.CreationDate, i.StartDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName,  " +
                "i.Room, i.Quantity, i.ForSale " +
                "FROM Inventory i, Person p " +
                "WHERE i.AuthorId = p.Id " +
                "AND lower(i.Brand) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                "ORDER BY i.CreationDate DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[8].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                        result.Entity.Name = "Inventory";
                        result.SubType = dr[1].ToString()!;
                        result.FieldName = "Brand";
                        result.SearchText = dr[6].ToString()!;

                        results.Add(result);
                    }
                }
            }

            sql = "SELECT i.Id, i.Description, i.IGroup, i.IType, i.OriginalValue, i.CurrentValue, " +
                  "i.Brand, i.Notes, i.CreationDate, i.StartDate, i.EndDate, i.LastUpdated, i.AuthorId, p.FirstName, p.LastName,  " +
                  "i.Room, i.Quantity, i.ForSale " +
                  "FROM Inventory i, Person p " +
                  "WHERE i.AuthorId = p.Id " +
                  "AND lower(i.Notes) LIKE '%" + searchText.ToLowerInvariant() + "%' " +
                  "ORDER BY i.CreationDate DESC";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var result = new SearchResult();
                        result.AsOfDate = Convert.ToDateTime(dr[8].ToString());
                        result.IsCurrent = true;
                        result.Entity.Id = Convert.ToInt64(dr[0].ToString());
                        result.Entity.Name = "Inventory";
                        result.SubType = dr[1].ToString()!;
                        result.FieldName = "Notes";
                        result.SearchText = dr[7].ToString()!;

                        results.Add(result);
                    }
                }

            }
            
            return results;
            
        }
    
    public static List<string> GetAllBrands(string conn)
    {
        try
        {
            var brands = new List<string>();
            Inventory inv;

            string sqlQuery =
                "SELECT DISTINCT(i.Brand) " +
                "FROM Inventory i ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (!brands.Contains(dr[0].ToString().Trim()))
                        {
                            brands.Add(dr[0].ToString().Trim());
                        }
                    }
                }
            }

            return brands;
        }
        catch
        {
            return new List<string>();
        }
    }
    
    public static List<string> GetAllRooms(string conn)
    {
        try
        {
            var objs = new List<string>();
            Inventory inv;

            string sqlQuery =
                "SELECT DISTINCT(i.Room) " +
                "FROM Inventory i ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (!objs.Contains(dr[0].ToString().Trim()))
                        {
                            objs.Add(dr[0].ToString().Trim());
                        }
                    }
                }
            }

            return objs;
        }
        catch
        {
            return new List<string>();
        }
    }
    
    public static List<string> GetAllGroups(string conn)
    {
        try
        {
            var objs = new List<string>();
            Inventory inv;

            string sqlQuery =
                "SELECT DISTINCT(i.Igroup) " +
                "FROM Inventory i ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (!objs.Contains(dr[0].ToString().Trim()))
                        {
                            objs.Add(dr[0].ToString().Trim());
                        }
                    }
                }
            }

            return objs;
        }
        catch
        {
            return new List<string>();
        }
    }
    
    public static List<string> GetAllTypes(string conn)
    {
        try
        {
            var objs = new List<string>();
            Inventory inv;

            string sqlQuery =
                "SELECT DISTINCT(i.Itype) " +
                "FROM Inventory i ";

            using (IDbConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (!objs.Contains(dr[0].ToString().Trim()))
                        {
                            objs.Add(dr[0].ToString().Trim());
                        }
                    }
                }
            }

            return objs;
        }
        catch
        {
            return new List<string>();
        }
    }
    
    public static bool Insert(Inventory inventory)
    {
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
            {
                string sqlQuery = "INSERT INTO Inventory (Description, StartDate, EndDate, CreationDate, IType, IGroup, AuthorId, OriginalValue," +
                    "CurrentValue, Brand, Notes, Room, Quantity, ForSale, LastUpdated) " +
                    "VALUES(@Description, @StartDate, @EndDate, CURRENT_DATE, @InventoryType, @InventoryGroup, @AuthorId, @OriginalValue, @CurrentValue, " +
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
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery =
                        "UPDATE Inventory SET Description = @Description, StartDate = @StartDate, EndDate = @EndDate, IType = @InventoryType, " +
                        "IGroup = @InventoryGroup, AuthorId = @AuthorId, OriginalValue = @OriginalValue, CurrentValue = @CurrentValue, " +
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
            
    public static List<InventoryObservation> GetAllObservationsForInventoryItem(string connectionString, long inventoryId)
    {
        {
            var myObs = new List<InventoryObservation>();
            InventoryObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.InventoryId, pp.Description " +
                      "FROM InventoryObservation o, CommentType ct, Person p, Inventory pp " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.InventoryId = pp.Id " +
                      "AND o.InventoryId = @Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", inventoryId);
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new InventoryObservation();
                        
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
                        o.Inventory.Id = Convert.ToInt64(dr[10].ToString());
                        o.Inventory.Description = dr[11].ToString();
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
    
    public static bool InsertInventoryObservation(string connectionString, InventoryObservation obs)
    {
        var rtnValue = false;

        var sql = "INSERT INTO InventoryObservation (InventoryId, Comment, CreationDate, StartDate, EndDate, CommentTypeId, AuthorId) " +
                  "VALUES(@inventoryId, @comment, CURRENT_DATE, CURRENT_DATE, '9999-12-31', @commentTypeId, @authorId) ";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("@inventoryId", obs.InventoryId);
                command.Parameters.AddWithValue("@comment", obs.Comment);
                command.Parameters.AddWithValue("@commentTypeId", obs.CommentType!.Id);
                command.Parameters.AddWithValue("@authorId", obs.Author!.Id);

                rtnValue = (command.ExecuteNonQuery() == 1);
            }
        }

        return rtnValue;
    }
    
    public static List<InventoryObservation> GetAllInventoryObservations(string connectionString)
    {
        {
            var myObs = new List<InventoryObservation>();
            InventoryObservation o;

            var sql = "SELECT o.Comment, o.CreationDate, o.StartDate, o.EndDate, o.CommentTypeId, " +
                      "ct.Description, o.AuthorId, p.FirstName, p.LastName, o.Id, o.PersonId, pp.Description " +
                      "FROM InventoryObservation o, CommentType ct, Person p, Inventory pp " +
                      "WHERE ct.Id = o.CommentTypeId " +
                      "AND o.InventoryId = pp.Id " +
                      "AND p.Id = o.AuthorId ORDER BY o.Id DESC";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        o = new InventoryObservation();
                        
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
                        o.Inventory.Id = Convert.ToInt64(dr[10].ToString());
                        o.Inventory.Description = dr[11].ToString();
                        
                        o.AsOfDate = o.CreationDate;

                        myObs.Add(o);
                    }
                }

                return myObs;
            }
        }
    }
}