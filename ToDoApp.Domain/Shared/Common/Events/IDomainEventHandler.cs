using System.Threading.Tasks;

namespace ToDoApp.Domain.Shared.Common.Events
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent);
    }
}