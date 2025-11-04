using System;
using System.Data;
using System.Collections.Generic;
//using System.Drawing;

using Avalonia.Media.Imaging;

using System.Net.Mime;
using System.Text;

using Npgsql;
using Dapper;
using Image = Models.Image;

namespace DataAccess.Server;

public class ImageStoreRepository
{
    public static Image LoadImageFromFile(string fileName)
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

    public static Bitmap LoadImageFromDatabase(long id)
    {
        Bitmap myImage = null;
        
        var sql = "SELECT i.ImageBlob " + 
                  "FROM ImageStore i " + 
                  "WHERE i.Id = @Id";

        using (IDbConnection connection = new NpgsqlConnection(DataConnection.GetServerDataSource()))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("@Id", id));
               
                var dr = command.ExecuteReader();

                byte[] imageByte = null;
                
                if (dr.Read())
                {
                    imageByte = (byte[])dr[0];
                }
                dr.Close();
                
                if (imageByte != null)
                {
                    using (MemoryStream productImageStream = new System.IO.MemoryStream(imageByte))
                    {
                        myImage =  new Bitmap(productImageStream);
                    }
                }
                
                // while (dr.Read())
                // {
                //     var imageString = dr.GetString(0);
                //     //var stream = new Stream(imageString);
                //     //BinaryReader br = new BinaryReader(stream);
                //     //byte[] data = br.ReadBytes();
                //     myImage.ImageBlob = byte[](dr.GetString(0));
                //     myImage.Id = Convert.ToInt64(dr[1].ToString());
                // }
            }
        }
        return myImage;
    }

    public static Image SaveImage(string fileName)
    {
        var image = LoadImageFromFile(fileName);
        
        try
        {
            using (IDbConnection db = new NpgsqlConnection(DataConnection.GetServerDataSource()))
            {
                string sqlQuery = "INSERT INTO ImageStore (FileName, ImageGroupId, ImageBlob, CreationDate) " +
                                  "VALUES(@FileName, 0, @ImageBlob, CURRENT_DATE);";

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

        using (IDbConnection connection = new NpgsqlConnection(DataConnection.GetServerDataSource()))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("@FileName", fileName));
                command.Parameters.Add(new NpgsqlParameter("@CreationDate", creationDate));
                
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