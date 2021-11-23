using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems.ReadModel
{
    public interface IToDoItemReadRepository
    {
        Task<ToDoItem> GetById(UserId userId, Guid id);

        Task<IEnumerable<ToDoItem>> GetAllForUser(UserId userId);
    }
}
