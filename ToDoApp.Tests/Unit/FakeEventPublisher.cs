using System.Threading.Tasks;
using ToDoApp.Domain.Shared.Common.Events;

namespace ToDoApp.Tests.Unit
{
    internal class FakeEventPublisher : IDomainEventPublisher
    {
        public Task Publish<T>(T domainEvent) where T : IDomainEvent => Task.CompletedTask;
    }
}
