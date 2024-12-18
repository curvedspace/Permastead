using DataAccess;
using Models;

namespace Services;

public class MeasurementsService
{
    public static List<MeasurementUnit> GetAllMeasurementTypes(ServiceMode mode)
    {
        var items = new List<MeasurementUnit>();

        if (mode == ServiceMode.Local)
        {
            //myAnimals = HarvestRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            items = DataAccess.Server.MeasurementUnitRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return items;
    }
}