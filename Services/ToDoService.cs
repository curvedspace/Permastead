using DataAccess.Local;
using DataAccess;

using Models;

namespace Services
{
    public class ToDoService
    {
        public static List<ToDo> GetAllToDos(ServiceMode mode)
        {
            var todos = new List<ToDo>();

            if (mode == ServiceMode.Local)
            {
                todos = ToDoRepository.GetAllToDos(DataConnection.GetLocalDataSource());
            }
            else
            {
                todos = DataAccess.Server.ToDoRepository.GetAllToDos(DataConnection.GetServerConnectionString());
            }

            return todos;
        }
        
        public static List<ToDo> GetActiveTodos(ServiceMode mode)
        {
            var todos = new List<ToDo>();

            if (mode == ServiceMode.Local)
            {
                todos = ToDoRepository.GetActiveToDos(DataConnection.GetLocalDataSource());
            }
            else
            {
                todos = DataAccess.Server.ToDoRepository.GetActiveToDos(DataConnection.GetServerConnectionString());
            }

            return todos;
        }

        public static List<ToDo> GetUpcomingToDos(ServiceMode mode, long daysInAdvance)
        {
            var todos = new List<ToDo>();

            if (mode == ServiceMode.Local)
            {
                todos = ToDoRepository.GetUpcomingToDos(DataConnection.GetLocalDataSource(), daysInAdvance);
            }
            else
            {
                todos = DataAccess.Server.ToDoRepository.GetUpcomingToDos(DataConnection.GetServerConnectionString(), daysInAdvance);
            }

            return todos;
        }
        
        public static List<ToDoType> GetAllToDoTypes(ServiceMode mode)
        {
            var todoTypes = new List<ToDoType>();

            if (mode == ServiceMode.Local)
            {
                todoTypes = ToDoTypeRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                todoTypes = DataAccess.Server.ToDoTypeRepository.GetAll(DataConnection.GetServerConnectionString());
            }

            return todoTypes;
        }
        
        public static List<ToDoStatus> GetAllToDoStatuses(ServiceMode mode)
        {
            var todoStatuses = new List<ToDoStatus>();

            if (mode == ServiceMode.Local)
            {
                todoStatuses = ToDoStatusRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                todoStatuses = DataAccess.Server.ToDoStatusRepository.GetAll(DataConnection.GetServerConnectionString());
            }

            return todoStatuses;
        }

        public static bool DoesToDoExist(ServiceMode mode, string? description)
        {
            if (mode == ServiceMode.Local)
            {
                return ToDoRepository.DoesToDoExist(description);
            }
            else
            {
                return DataAccess.Server.ToDoRepository.DoesToDoExist(description);
            }
            
        }

        public static bool CommitRecord(ServiceMode mode, ToDo? todo)
        {
            bool rtnValue = false;
        
            if (todo != null)
            {
                if (todo.Id > 0)
                {
                    if (mode == ServiceMode.Local)
                    {
                        ToDoRepository.Update(todo);
                    }
                    else
                    {
                        DataAccess.Server.ToDoRepository.Update(todo);
                    }
                }
                else
                {
                    // insert new record
                    if (mode == ServiceMode.Local)
                    {
                        ToDoRepository.Insert(todo);
                    }
                    else
                    {
                        DataAccess.Server.ToDoRepository.Insert(todo);
                    }
                }
            }

            return rtnValue;
        }
    }
}
