using System;

namespace ToDoApp.Domain.Shared.Common.Events
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
    }
}