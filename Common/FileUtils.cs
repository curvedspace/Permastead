namespace Common;

public class FileUtils
{
    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public static bool DirectoryExists(string path, bool createIfNotExists = false)
    {
        var exists = Directory.Exists(path);
        
        if (!exists && createIfNotExists)
        {
            Directory.CreateDirectory(path);
        }
        
        return exists;
    }
}