using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemInMemoryRepository : IToDoItemRepository
    {
        private readonly Dictionary<ToDoItemId, ToDoItem> _toDoItems = new Dictionary<ToDoItemId, ToDoItem>();

        public Task CreateNew(ToDoItem sub) => Update(sub);

        public Task Update(ToDoItem toDoItem)
        {
            _toDoItems[toDoItem.Id] = toDoItem;
            return Task.CompletedTask;
        }

        public Task<ToDoItem> FindById(ToDoItemId id) => Task.FromResult(_toDoItems
            .Values.FirstOrDefault(toDoItem => toDoItem.Id.Equals(id)));

        public IEnumerable<ToDoItem> All() => _toDoItems.Values;

        public Task Remove(ToDoItemId id)
        {
            _toDoItems.Remove(id);
            return Task.CompletedTask;
        }
    }
}
