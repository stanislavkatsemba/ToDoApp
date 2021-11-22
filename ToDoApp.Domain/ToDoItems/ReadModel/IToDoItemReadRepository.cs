using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApp.Domain.ToDoItems.ReadModel
{
    public interface IToDoItemReadRepository
    {
        Task<ToDoItem> GetById(Guid userId, Guid id);

        Task<IEnumerable<ToDoItem>> GetAllForUser(Guid userId);
    }
}
