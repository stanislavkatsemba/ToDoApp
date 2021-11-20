using System.Threading.Tasks;
using ToDoApp.Data.Exceptions;
using ToDoApp.Data.Models;
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
            await _databaseContext.ToDoItems.AddAsync(new ToDoItemModel(toDoItem.ToSnapshot()));
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<ToDoItem> FindBy(ToDoItemId id)
        {
            var snapshot = await _databaseContext.ToDoItems.FindAsync(id.Value.ToString());
            return snapshot != null ? ToDoItem.FromSnapshot(snapshot) : null;
        }

        public async Task Update(ToDoItem toDoItem)
        {
            var snapshot = toDoItem.ToSnapshot();
            var dbItem = await _databaseContext.ToDoItems.FindAsync(snapshot.Id);
            if (dbItem != null)
            {
                _databaseContext.Entry(dbItem).CurrentValues.SetValues(snapshot);
                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException(nameof(ToDoItemId), snapshot.Id);
            }
        }
    }
}
