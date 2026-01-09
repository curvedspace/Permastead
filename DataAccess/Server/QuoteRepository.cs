using Npgsql;

using System.Data;

using Models;
using Dapper;

namespace DataAccess.Server
{
    public static class QuoteRepository
    {
        /// <summary>
        /// Gets a random quote from the local database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>A Quote object.</returns>
        public static Quote GetRandomQuote(string connectionString)
        {
            var q = new Quote();

            var sql = $"SELECT id, description, authorname FROM quote WHERE (enddate IS NULL OR enddate > CURRENT_DATE) ORDER BY RANDOM() LIMIT 1";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;  
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        q.Id = Convert.ToInt32(dr["id"]);
                        q.Description = dr["description"].ToString()!;
                        q.AuthorName = dr["authorname"].ToString()!;
                    }
                }

                return q;
            }
        }

        public static DataTable GetAll(string connectionString)
        {
            var dt = new DataTable();
            var sql = $"SELECT * FROM quote ";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;  
                    var dr = command.ExecuteReader();
                    dt.Load(dr);
                }

                return dt;
            }
        }
        
        public static bool Insert(string connectionString, Quote quote)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(connectionString))
                {
                    string sqlQuery = $"INSERT INTO Quote (Description, AuthorName, CreationDate, StartDate, EndDate) " + 
                        "VALUES(@Description,@AuthorName,CURRENT_DATE,@StartDate,@EndDate);";

                    return (db.Execute(sqlQuery, quote) ==1);
                }
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Delete(string connectionString, Quote quote)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerConnectionString()))
                {
                    string sqlQuery = "UPDATE Quote SET EndDate = @EndDate WHERE Id = @Id;";

                    return (db.Execute(sqlQuery, quote) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
