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
    
    public static List<string> GetAllCompanies(ServiceMode mode)
    {
        var companies = new List<string>();

        if (mode == ServiceMode.Local)
        {
            companies = PersonRepository.GetAllCompanies(DataConnection.GetLocalDataSource());
        }

        return companies;
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

    public static bool CommitRecord(ServiceMode serviceMode, Person person)
    {
        bool rtnValue = false;
        
        if (person != null)
        {
            if (person.Id > 0)
            {
                PersonRepository.Update(person);
            }
            else
            {
                // insert new record
                PersonRepository.Insert(person);
            }
        }

        return rtnValue;
    }
}