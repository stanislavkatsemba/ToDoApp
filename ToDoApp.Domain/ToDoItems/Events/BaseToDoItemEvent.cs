using System;
using ToDoApp.Domain.Shared.Common.Events;
using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems.Events
{
    public abstract class BaseToDoItemEvent : IDomainEvent
    {
        public Guid EventId { get; }

        public ToDoItemId ToDoItemId { get; }

        public UserId UserId { get; }

        protected BaseToDoItemEvent(UserId userId, ToDoItemId toDoItemId)
        {
            EventId = Guid.NewGuid();
            ToDoItemId = toDoItemId;
            UserId = userId;
        }
    }
}
