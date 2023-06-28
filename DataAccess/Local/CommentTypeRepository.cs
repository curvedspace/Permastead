﻿using Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Dapper;

namespace DataAccess.Local
{
    public class CommentTypeRepository
    {
        public static bool Insert(CommentType commentType)
        {
            try
            {
                using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
                {
                    string sqlQuery = "INSERT INTO CommentType (Code, Description, CreationDate, StartDate, EndDate, AuthorId) " +
                        "VALUES(@Code, @Description, CURRENT_DATE, @StartDate, @EndDate, @AuthorId);";

                    return (db.Execute(sqlQuery, commentType) == 1);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
