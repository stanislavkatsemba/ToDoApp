using System.Threading.Tasks;

namespace ToDoApp.Domain.Shared.Common.Events
{
    public interface IDomainEventPublisher
    {
        Task Publish<T>(T domainEvent) where T : IDomainEvent;
    }
}
