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
}