using DataAccess.Local;
using DataAccess;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
