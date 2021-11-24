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

        public async Task<ToDoItem> FindById(ToDoItemId id)
        {
            var snapshot = await _databaseContext.ToDoItems.FindAsync(id.Value.ToString());
            return snapshot != null ? ToDoItem.FromSnapshot(snapshot) : null;
        }

        public async Task Remove(ToDoItemId id)
        {
            var dbItem = await FindByIdPrivate(id.Value.ToString());
            _databaseContext.Remove(dbItem);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task Update(ToDoItem toDoItem)
        {
            var snapshot = toDoItem.ToSnapshot();
            var dbItem = await FindByIdPrivate(snapshot.Id);
            _databaseContext.Entry(dbItem).CurrentValues.SetValues(snapshot);
            _databaseContext.Update(dbItem);
            await _databaseContext.SaveChangesAsync();
        }

        private async Task<ToDoItemModel> FindByIdPrivate(string id, bool throwExceptionIfNull = true)
        {
            var toDoItem = await _databaseContext.ToDoItems.FindAsync(id);
            if (toDoItem == null && throwExceptionIfNull)
            {
                throw new EntityNotFoundException(nameof(ToDoItemId), id);
            }
            return toDoItem;
        }
    }
}
