using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DataAccess.Local;
using DataAccess;

using Models;

namespace Services
{
    public class PlantService
    {
        public static List<Plant> GetAllPlants(ServiceMode mode)
        {
            var plants = new List<Plant>();

            if (mode == ServiceMode.Local)
            {
                plants = PlantRepository.GetPlants(DataConnection.GetLocalDataSource());
            }
            else
            {
                plants = DataAccess.Server.PlantRepository.GetPlants(DataConnection.GetServerConnectionString());
            }

            return plants;
        }
        
        public static Plant GetPlantFromId(ServiceMode mode, long id) 
        {
            var plant = new Plant();

            if (mode == ServiceMode.Local)
            {
                plant = PlantRepository.GetPlantFromId(DataConnection.GetLocalDataSource(), id);
            }
            else
            {
                plant = DataAccess.Server.PlantRepository.GetPlantFromId(DataConnection.GetServerConnectionString(), id);
            }

            return plant;
        }
        
        public static bool CommitRecord(ServiceMode serviceMode, Plant? plant)
        {
            bool rtnValue = false;
        
            if (plant != null)
            {
                if (plant.Id > 0)
                {
                    if (serviceMode == ServiceMode.Local)
                        PlantRepository.Update(plant);
                    else
                    {
                        DataAccess.Server.PlantRepository.Update(plant);
                    }
                }
                else
                {
                    // insert new record
                    if (serviceMode == ServiceMode.Local)
                        PlantRepository.Insert(plant);
                    else
                    {
                        DataAccess.Server.PlantRepository.Insert(plant);
                    }
                }
            }

            return rtnValue;
        }

        public static bool DeleteRecord(ServiceMode serviceMode, Plant? plant)
        {
            if (plant != null)
            {
                
                if (serviceMode == ServiceMode.Local)
                {
                    plant.EndDate = DateTime.Now;
                    return CommitRecord(serviceMode, plant);
                }
                else
                {
                    plant.EndDate = DateTime.Now;
                    return CommitRecord(serviceMode, plant);
                }
            }
            else
            {
                return false;
            }
            
        }

        public static Bitmap GetPlantImage(Plant? plant)
        {
            if (plant == null)
            {
                AssetLoader.GetAssets(new Uri("avares://Permastead"),null);
                return new Bitmap(AssetLoader.Open(new Uri("avares://Permastead/Assets/plant_icon.png"), null));
            }
            else
            {
                //get image location
                var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                if (isWindows)
                {
                    var newLocation = userFolder + @"\.config\permastead\images\plants\" + plant.Id + ".png";
                    FileInfo fi = new FileInfo(newLocation);

                    if (fi.Exists)
                    {
                        {
                            using var stream = fi.OpenRead();
                            using (var streamReader = new StreamReader(stream))
                            {
                                Bitmap pic = new Bitmap(stream);
                                return pic;
                            }
                        }
                    }
                    else
                    {
                        AssetLoader.GetAssets(new Uri("avares://Permastead"),null);
                        return new Bitmap(AssetLoader.Open(new Uri("avares://Permastead/Assets/plant_icon.png"), null));
                    }
                    
                }
                else
                {
                    var newLocation = userFolder + @"/.config/permastead/images/plants/" + plant.Id + ".png";
                    FileInfo fi = new FileInfo(newLocation);

                    if (fi.Exists)
                    {
                        using var stream = fi.OpenRead();
                        using (var streamReader = new StreamReader(stream))
                        {
                            Bitmap pic = new Bitmap(stream);
                            return pic;
                        }
                    }
                    else
                    {
                        //use the default plant photo
                        AssetLoader.GetAssets(new Uri("avares://Permastead"),null);
                        return new Bitmap(AssetLoader.Open(new Uri("avares://Permastead/Assets/plant_icon.png"), null));
                    }
                }
                
            }
        }
    }
}
