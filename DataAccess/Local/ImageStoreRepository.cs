using System;
using System.Data;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

using Dapper;

using Microsoft.Data.Sqlite;

using Models;

namespace DataAccess.Local;

public class ImageStoreRepository
{

    public static Image LoadImage(string fileName)
    {
        //Create an instance of the Image Class/Object
        //so that we can store the information
        //about the picture an send it back for
        //processing into the database.
        Image? image = null;
        int maxImageSize = 2097152;

        if (fileName == null || fileName == string.Empty)
            return image;

        if (fileName != string.Empty && fileName != null)
        {
            
            //Get file information and calculate the filesize
            FileInfo info = new FileInfo(fileName);
            long fileSize = info.Length;

            //re-assign the filesize to calculated filesize
            maxImageSize = (Int32)fileSize;

            if (File.Exists(fileName))
            {
                //Retrieve image from file and binary it to Object image
                using (FileStream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(stream);
                    byte[] data = br.ReadBytes(maxImageSize);
                    image = new Image(fileName, data, fileSize);
                }
            }
        }
        return image;
    }
    
    public static Stream LoadImageStream(string fileName)
    {
        //Create an instance of the Image Class/Object
        //so that we can store the information
        //about the picture an send it back for
        //processing into the database.
        
        int maxImageSize = 2097152;

        if (fileName == null || fileName == string.Empty)
            return null;

        if (fileName != string.Empty && fileName != null)
        {
            
            //Get file information and calculate the filesize
            FileInfo info = new FileInfo(fileName);
            long fileSize = info.Length;

            //re-assign the filesize to calculated filesize
            maxImageSize = (Int32)fileSize;

            if (File.Exists(fileName))
            {
                //Retrieve image from file and binary it to Object image
                FileStream stream = File.Open(fileName, FileMode.Open);
                return stream;
            }
        }

        return null;
    }

    public static Image SaveImage(string fileName)
    {
        Image image = LoadImage(fileName);
        
        try
        {
            using (IDbConnection db = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                string sqlQuery = "INSERT INTO ImageStore (FileName, ImageBlob, FileSize, CreationDate) " +
                                  "VALUES(@FileName, @ImageBlob, @FileSize, CURRENT_DATE);";
        
                
                if (db.Execute(sqlQuery, image) == 1)
                {
                    //get the image ID of the new record...dapper does not populate it
                    image.Id = GetImageId(image.FileName, DateTime.Today);
                    
                    return image;
                }
                else
                {
                    return null;
                }
            }
        }
        catch
        {
            return image;
        }
    }
    
    private static long GetImageId(string fileName, DateTime creationDate)
    {
        long id = 0;
        var sql = "SELECT i.Id " + 
                  "FROM ImageStore i " + 
                  "WHERE i.FileName = @FileName " +
                  "AND i.CreationDate  = @CreationDate ";

        using (IDbConnection connection = new SqliteConnection(DataConnection.GetLocalDataSource()))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SqliteParameter("@FileName", fileName));
                command.Parameters.Add(new SqliteParameter("@CreationDate", creationDate));
                
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    id = Convert.ToInt64(dr[0].ToString());
                }
            }
        }
        return id;
    }
}