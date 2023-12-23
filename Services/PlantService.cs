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

            return plants;
        }
        
        public static Plant GetPlantFromId(ServiceMode mode, long id) 
        {
            var plant = new Plant();

            if (mode == ServiceMode.Local)
            {
                plant = PlantRepository.GetPlantFromId(DataConnection.GetLocalDataSource(), id);
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
                    PlantRepository.Update(plant);
                }
                else
                {
                    // insert new record
                    PlantRepository.Insert(plant);
                }
            }

            return rtnValue;
        }
    }
}
