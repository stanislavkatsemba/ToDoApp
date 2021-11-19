using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemInMemoryRepository : IToDoItemRepository
    {
        private readonly Dictionary<ToDoItemId, ToDoItem> _totDoItems = new Dictionary<ToDoItemId, ToDoItem>();

        public Task CreateNew(ToDoItem sub) => Update(sub);

        public Task Update(ToDoItem toDoItem)
        {
            _totDoItems[toDoItem.Id] = toDoItem;
            return Task.CompletedTask;
        }

        public Task<ToDoItem> FindBy(ToDoItemId id) => Task.FromResult(_totDoItems
            .Values.FirstOrDefault(toDoItem => toDoItem.Id.Equals(id)));

        public IEnumerable<ToDoItem> All() => _totDoItems.Values;
    }
}
