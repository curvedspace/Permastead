using Avalonia.Media.Imaging;
using DataAccess;
using DataAccess.Local;
using Models;

namespace Services;

 public class ImageHelperService
{
    public static Image? SaveImage(ServiceMode mode, string fileName)
    {
        Image? rtnValue = new Image();
        
        if (fileName != string.Empty)
        {
            if (mode == ServiceMode.Local)
            {
                rtnValue = DataAccess.Local.ImageStoreRepository.SaveImage(fileName);
            }
            else
            {
                rtnValue = DataAccess.Server.ImageStoreRepository.SaveImage(fileName);
            }
        }
        
        return rtnValue;
    }
    
    
    public static Avalonia.Media.Imaging.Bitmap GetImage(ServiceMode mode, long id)
    {
        Bitmap rtnValue = null;
        
        if (id > 0)
        {
            if (mode == ServiceMode.Local)
            {
                //rtnValue = DataAccess.Local.ImageStoreRepository.LoadImageFromDatabase(id);
            }
            else
            {
                rtnValue = DataAccess.Server.ImageStoreRepository.LoadImageFromDatabase(id);
            }
        }
        
        return rtnValue;
    }

    
    
}
