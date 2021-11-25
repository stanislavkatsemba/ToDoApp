using System;
using ToDoApp.Domain.Shared.Common.Events;
using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemChanged : IDomainEvent
    {
        public Guid EventId { get; }

        public ToDoItemId ToDoItemId { get; }
        
        public UserId UserId { get; }

        public ToDoItemChanged(UserId userId, ToDoItemId toDoItemId)
        {
            EventId = Guid.NewGuid();
            ToDoItemId = toDoItemId;
            UserId = userId;
        }
    }
}
