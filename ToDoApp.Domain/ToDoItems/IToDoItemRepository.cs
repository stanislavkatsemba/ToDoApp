using System.Threading.Tasks;

namespace ToDoApp.Domain.ToDoItems
{
    public interface IToDoItemRepository
    {
        Task CreateNew(ToDoItem toDoItem);

        Task Update(ToDoItem toDoItem);

        Task<ToDoItem> FindById(ToDoItemId id);

        Task Remove(ToDoItemId id);
    }
}
