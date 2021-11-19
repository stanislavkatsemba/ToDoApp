using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Web;
using Xunit;
using static ToDoApp.Tests.Fixtures.ToDoItemFixture;
using static ToDoApp.Tests.Fixtures.UserFixture;

namespace ToDoApp.Tests.Integration.ToDoItems
{
    public class ToDoItemRepositoryTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IToDoItemRepository _repository;

        public ToDoItemRepositoryTests(WebApplicationFactory<Startup> appFactory)
        {
            _scope = appFactory.Services.CreateScope();
            _repository = _scope.ServiceProvider.GetService<IToDoItemRepository>();
        }

        [Fact]
        public async Task CanSaveAToDoItem()
        {
            // given:
            var userId = CreateAnyUser();
            // and:
            var toDoItem = CreateToDoItemOwnedBy(userId);
            // and:
            await _repository.CreateNew(toDoItem);
            // and:
            toDoItem.Complete();
            // and:
            await _repository.Update(toDoItem);
            // and:
            var toDoItemFromDb = await _repository.FindBy(toDoItem.Id);
            // expect:
            toDoItemFromDb.Should().NotBeNull();
            // and:
            toDoItemFromDb.Id.Should().Be(toDoItem.Id);
            toDoItemFromDb.IsOwnedBy(userId).Should().BeTrue();
            toDoItemFromDb.IsCompleted.Should().BeTrue();
        }

        public void Dispose() => _scope.Dispose();
    }
}
