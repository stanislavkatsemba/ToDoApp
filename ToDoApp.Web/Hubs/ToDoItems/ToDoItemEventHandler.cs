using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ToDoApp.Domain.Shared.Common.Events;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.ToDoItems.ReadModel;

namespace ToDoApp.Web.Hubs.ToDoItems
{
    public class ToDoItemEventHandler : IDomainEventHandler<ToDoItemChanged>, IDomainEventHandler<ToDoItemRemoved>
    {
        private readonly IHubContext<ToDoItemHub, IToDoItemHub> _toDoItemHub;
        private readonly IToDoItemReadRepository _toDoItemReadSqlRepository;
        public ToDoItemEventHandler(IHubContext<ToDoItemHub, IToDoItemHub> toDoItemHub, IToDoItemReadRepository toDoItemReadSqlRepository)
        {
            _toDoItemHub = toDoItemHub;
            _toDoItemReadSqlRepository = toDoItemReadSqlRepository;
        }

        public async Task Handle(ToDoItemChanged changedItem)
        {
            var item = await _toDoItemReadSqlRepository.GetById(changedItem.UserId, changedItem.ToDoItemId);
            await _toDoItemHub.Clients.Users(changedItem.UserId.Value.ToString()).ReceiveToDoItem(item);
        }

        public async Task Handle(ToDoItemRemoved removedItem)
        {
            await _toDoItemHub.Clients.Users(removedItem.UserId.Value.ToString()).ToDoItemRemoved(removedItem.ToDoItemId.Value.ToString());
        }
    }
}