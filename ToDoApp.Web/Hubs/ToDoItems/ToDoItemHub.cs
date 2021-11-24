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
    public interface IChatClient
    {
        Task ReceiveToDoItem(ToDoItemRead message);
        Task ToDoItemRemoved(string toDoItemId);
    }

    [Authorize]
    public class ToDoItemHub : Hub<IChatClient>
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
            var result = await _toDoItemService.Create(newToDoItem);
            
            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, newToDoItem.Id.Value);
                await Clients.User(userId.Value.ToString()).ReceiveToDoItem(item);
            }
        }

        public async Task UpdateToDoItem(ToDoItemDto toDoItemDto)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(toDoItemDto.Id));
            var result = await _toDoItemService.Update(userId, toDoItemId, toDoItemDto.Name, toDoItemDto.Description);

            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, toDoItemId.Value);
                await Clients.User(userId.Value.ToString()).ReceiveToDoItem(item);
            }
        }

        public async Task ScheduleDoItem(string id, DateTime date)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            var result = await _toDoItemService.Schedule(userId, toDoItemId, date);

            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, toDoItemId.Value);
                await Clients.User(userId.Value.ToString()).ReceiveToDoItem(item);
            }
        }

        public async Task CompleteToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            var result = await _toDoItemService.Complete(userId, toDoItemId);

            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, toDoItemId.Value);
                await Clients.User(userId.Value.ToString()).ReceiveToDoItem(item);
            }
        }

        public async Task RevokeCompletionToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var toDoItemId = new ToDoItemId(new Guid(id));
            var result = await _toDoItemService.RevokeCompletion(userId, toDoItemId);

            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, toDoItemId.Value);
                await Clients.User(userId.Value.ToString()).ReceiveToDoItem(item);
            }
        }

        public async Task RemoveToDoItem(string id)
        {
            var userId = Context.User.GetUserId();
            var result = await _toDoItemService.Remove(userId, new ToDoItemId(new Guid(id)));

            //TODO: separate event
            if (result.IsSuccessful)
            {
                await Clients.User(userId.Value.ToString()).ToDoItemRemoved(id);
            }
        }

        public async Task<IEnumerable<ToDoItemRead>> GetAllToDoItems()
        {
            var userId = Context.User.GetUserId();
            var result = await _toDoItemReadSqlRepository.GetAllForUser(userId);
            return result;
        }
    }
}
