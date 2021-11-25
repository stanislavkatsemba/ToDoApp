using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.ToDoItems.ReadModel;
using ToDoApp.Web.Common.Authentication;
using ToDoItem = ToDoApp.Domain.ToDoItems.ToDoItem;
using ToDoItemRead = ToDoApp.Domain.ToDoItems.ReadModel.ToDoItem;

namespace ToDoApp.Web.Hubs.ToDoItems
{
    [Authorize]
    public class ToDoItemHub : Hub<IToDoItemHub>
    {
        private readonly ToDoItemService _toDoItemService;
        private readonly IToDoItemReadRepository _toDoItemReadSqlRepository;

        public ToDoItemHub(IServiceProvider serviceProvider)
        {
            _toDoItemService = serviceProvider.GetRequiredService<ToDoItemService>();
            _toDoItemReadSqlRepository = serviceProvider.GetRequiredService<IToDoItemReadRepository>();
        }

        public async Task CreateToDoItem(ToDoItemDto toDoItemDto)
        {
            var userId = Context.User.GetUserId();
            var newToDoItem = ToDoItem.New(userId, toDoItemDto.Name, toDoItemDto.Description);
            await _toDoItemService.Create(newToDoItem);
        }

        public async Task UpdateToDoItem(ToDoItemDto toDoItemDto)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(toDoItemDto.Id));
            await _toDoItemService.Update(userId, toDoItemId, toDoItemDto.Name, toDoItemDto.Description);
        }

        public async Task ScheduleToDoItem(string id, DateTime date)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            await _toDoItemService.Schedule(userId, toDoItemId, date);
        }

        public async Task ClearSchedulingToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            await _toDoItemService.ClearScheduling(userId, toDoItemId);
        }

        public async Task CompleteToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            await _toDoItemService.Complete(userId, toDoItemId);
        }

        public async Task RevokeCompletionToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            await _toDoItemService.RevokeCompletion(userId, toDoItemId);
        }

        public async Task RemoveToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            await _toDoItemService.Remove(userId, new ToDoItemId(new Guid(id)));
        }

        public async Task<IEnumerable<ToDoItemRead>> GetAllToDoItems()
        {
            var userId = Context.User.GetUserId();
            var result = await _toDoItemReadSqlRepository.GetAllForUser(userId);
            return result;
        }
    }
}
