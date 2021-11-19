using System;

namespace ToDoApp.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, string id) : base($"{entityName} with id {id} was not found.")
        {

        }
    }
}
