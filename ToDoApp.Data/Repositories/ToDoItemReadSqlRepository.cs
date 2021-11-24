using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.Models;
using ToDoApp.Domain.ToDoItems.ReadModel;
using ToDoApp.Domain.Users;

namespace ToDoApp.Data.Repositories
{
    public class ToDoItemReadSqlRepository : IToDoItemReadRepository
    {
        private readonly ToDoAppContext _databaseContext;

        public ToDoItemReadSqlRepository(ToDoAppContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ToDoItem> GetById(UserId userId, Guid todItemId)
        {
            var command = SelectCommand +
                $"{nameof(ToDoItemModel.Id)} = {{0}} AND {nameof(ToDoItemModel.UserId)} = {{1}}";
            var result = await _databaseContext.ToDoItemsRead.FromSqlRaw(command, todItemId, userId.Value).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllForUser(UserId userId)
        {
            var command = SelectCommand + $@"{nameof(ToDoItemModel.UserId)} = {{0}}";
            var result = await _databaseContext.ToDoItemsRead.FromSqlRaw(command, userId.Value).ToListAsync();
            return result;
        }

        private static string _selectCommand;

        private static string SelectCommand
        {
            get
            {
                if (_selectCommand != null)
                {
                    return _selectCommand;
                }
                _selectCommand = $@"SELECT TOP 10000
    {nameof(ToDoItemModel.Id)}
    , {nameof(ToDoItemModel.Name)}
    , {nameof(ToDoItemModel.Description)}
    , {nameof(ToDoItemModel.CreationDate)}
    , {nameof(ToDoItemModel.ModificationDate)}
    , {nameof(ToDoItemModel.CompletionData)}
    , {nameof(ToDoItemModel.IsCompleted)}
    , {nameof(ToDoItemModel.ScheduledDate)}
FROM 
    {nameof(ToDoAppContext.ToDoItems)}
WHERE
    {nameof(ToDoItemModel.IsDeleted)} = 0
AND
";
                return _selectCommand;
            }
        }
    }
}
