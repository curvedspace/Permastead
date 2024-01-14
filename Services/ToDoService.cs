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

            return todos;
        }
        
        public static List<ToDo> GetActiveTodos(ServiceMode mode)
        {
            var todos = new List<ToDo>();

            if (mode == ServiceMode.Local)
            {
                todos = ToDoRepository.GetActiveToDos(DataConnection.GetLocalDataSource());
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

            return todos;
        }
        
        public static List<ToDoType> GetAllToDoTypes(ServiceMode mode)
        {
            var todoTypes = new List<ToDoType>();

            if (mode == ServiceMode.Local)
            {
                todoTypes = ToDoTypeRepository.GetAll(DataConnection.GetLocalDataSource());
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

            return todoStatuses;
        }

        public static bool DoesToDoExist(ServiceMode mode, string? description)
        {
            return ToDoRepository.DoesToDoExist(description);
        }

        public static bool CommitRecord(ServiceMode serviceMode, ToDo? todo)
        {
            bool rtnValue = false;
        
            if (todo != null)
            {
                if (todo.Id > 0)
                {
                    ToDoRepository.Update(todo);
                }
                else
                {
                    // insert new record
                    ToDoRepository.Insert(todo);
                }
            }

            return rtnValue;
        }
    }
}
