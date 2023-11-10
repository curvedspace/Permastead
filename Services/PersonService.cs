using DataAccess.Local;
using DataAccess;

using Models;

namespace Services;

public class PersonService
{
    public static List<Person> GetAllPeople(ServiceMode mode)
    {
        var people = new List<Person>();

        if (mode == ServiceMode.Local)
        {
            people = PersonRepository.GetAll(DataConnection.GetLocalDataSource());
        }

        return people;
    }
    
    public static Person GetPersonFromId(ServiceMode mode, long id) 
    {
        var person = new Person();

        if (mode == ServiceMode.Local)
        {
            person = PersonRepository.GetPersonFromId(DataConnection.GetLocalDataSource(), id);
        }

        return person;
    }
    
    public static List<PersonObservation> GetObservationsForPerson(ServiceMode mode, long id)
    {
        var obs = new List<PersonObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = PersonRepository.GetAllObservationsForPerson(DataConnection.GetLocalDataSource(), id);
        }

        return obs;
    }
}