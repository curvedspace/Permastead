using DataAccess;
using Models;
using DataAccess.Server;
using AnimalRepository = DataAccess.Local.AnimalRepository;

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
    
    public static List<AnimalType> GetAllAnimalTypes(ServiceMode mode)
    {
        var types = new List<AnimalType>();

        if (mode == ServiceMode.Local)
        {
            types = AnimalTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            types = DataAccess.Server.AnimalTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return types;
    }

    public static List<AnimalObservation> GetObservationsForAnimal(ServiceMode mode, long id)
    {
        var obs = new List<AnimalObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = AnimalRepository.GetAllObservationsForAnimal(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            obs = DataAccess.Server.AnimalRepository.GetAllObservationsForAnimal(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static List<AnimalObservation> GetAnimalObservations(ServiceMode mode)
    {
        var obs = new List<AnimalObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = AnimalRepository.GetAllAnimalObservations(DataConnection.GetLocalDataSource());
        }
        else
        {
            obs = DataAccess.Server.AnimalRepository.GetAllAnimalObservations(DataConnection.GetServerConnectionString());
        }

        return obs;
    }
    
    public static bool AddAnimalObservation(ServiceMode mode, AnimalObservation obs)
    {
        bool rtnValue = false;
        
        if (obs != null)
        {
            if (obs.Animal.Id > 0)
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    AnimalRepository.InsertAnimalObservation(
                        DataConnection.GetLocalDataSource(), obs);
                }
                else
                {
                    DataAccess.Server.AnimalRepository.InsertAnimalObservation(
                        DataConnection.GetServerConnectionString(), 
                        obs);
                }
            }
        }

        return rtnValue;
    }

    public static List<Entity> GetAnimalsAsEntityList(ServiceMode mode)
    {
        var items = new List<Entity>();
        var animals = GetAllAnimals(mode);

        foreach (var animal in animals)
        {
            var entity = new Entity();
            entity.Id = animal.Id;
            entity.Name = animal.Name;
            items.Add(entity);
        }

        return items;
    }
}