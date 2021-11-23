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

        public async Task CreateToDoItem(ToDoItemCreateInfo createInfo)
        {
            var userId = Context.User.GetUserId();
            var newToDoItem = ToDoItem.New(userId, createInfo.Name, createInfo.Description, createInfo.ScheduledDate);
            var result = await _toDoItemService.Create(newToDoItem);
            
            //TODO: separate event
            if (result.IsSuccessful)
            {
                var item = await _toDoItemReadSqlRepository.GetById(userId, newToDoItem.Id.Value);
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
