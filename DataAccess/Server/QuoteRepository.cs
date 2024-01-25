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

            var sql = $"SELECT description, authorname FROM quote WHERE rowid IN (SELECT rowid FROM quote " + 
                "WHERE enddate > CURRENT_DATE ORDER BY RANDOM() LIMIT 1)";

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;  
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                       
                        q.Description = dr[0].ToString()!;
                        q.AuthorName = dr[1].ToString()!;

                    }
                }

                return q;
            }
        }

        public static bool Insert(string connectionString, Quote quote)
        {
            try
            {
                using (IDbConnection db = new NpgsqlConnection(DataConnection.GetLocalDataSource()))
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
    }
}
