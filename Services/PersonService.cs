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
        else
        {
            people = DataAccess.Server.PersonRepository.GetAll(DataConnection.GetServerConnectionString());
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
        else
        {
            companies = DataAccess.Server.PersonRepository.GetAllCompanies(DataConnection.GetServerConnectionString());
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
        else
        {
            person = DataAccess.Server.PersonRepository.GetPersonFromId(DataConnection.GetServerConnectionString(), id);
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
        else
        {
            obs = DataAccess.Server.PersonRepository.GetAllObservationsForPerson(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static List<PersonObservation> GetAllPersonObservations(ServiceMode mode)
    {
        var obs = new List<PersonObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = PersonRepository.GetAllPersonObservations(DataConnection.GetLocalDataSource());
        }
        else
        {
            obs = DataAccess.Server.PersonRepository.GetAllPersonObservations(DataConnection.GetServerConnectionString());
        }

        return obs;
    }

    public static bool CommitRecord(ServiceMode mode, Person person)
    {
        bool rtnValue = false;
        
        if (person != null)
        {
            if (person.Id > 0)
            {
                if (mode == ServiceMode.Local)
                {
                    PersonRepository.Update(person);
                }
                else
                {
                    DataAccess.Server.PersonRepository.Update(person);
                }
                
            }
            else
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    PersonRepository.Insert(person);
                }
                else
                {
                    DataAccess.Server.PersonRepository.Insert(person);
                }
            }
        }

        return rtnValue;
    }
}