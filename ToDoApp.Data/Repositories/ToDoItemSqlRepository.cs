using System.Threading.Tasks;
using ToDoApp.Domain.ToDoItems;

namespace ToDoApp.Data.Repositories
{
    public class ToDoItemSqlRepository : IToDoItemRepository
    {
        private readonly ToDoAppContext _databaseContext;

        public ToDoItemSqlRepository(ToDoAppContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateNew(ToDoItem toDoItem)
        {
            await _databaseContext.ToDoItems.AddAsync(toDoItem.ToSnapshot());
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<ToDoItem> FindBy(ToDoItemId id)
        {
            var snapshot= await _databaseContext.ToDoItems.FindAsync(id.Value);
            return snapshot != null ? ToDoItem.FromSnapshot(snapshot) : null;
        }

        public async Task Update(ToDoItem toDoItem)
        {
            _databaseContext.ToDoItems.Update(toDoItem.ToSnapshot());
            await _databaseContext.SaveChangesAsync();
        }
    }
}
