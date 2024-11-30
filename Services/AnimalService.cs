using DataAccess;
using Models;
using DataAccess.Local;

namespace Services;

public class AnimalService
{
    public static List<Animal> GetAllAnimals(ServiceMode mode)
    {
        var myAnimals = new List<Animal>();

        if (mode == ServiceMode.Local)
        {
            myAnimals = AnimalRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            myAnimals = DataAccess.Server.AnimalRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return myAnimals;
    }
}