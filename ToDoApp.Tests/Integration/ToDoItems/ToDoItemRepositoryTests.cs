using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.Users;
using ToDoApp.Web;
using Xunit;
using static ToDoApp.Tests.Fixtures.ToDoItemFixture;
using static ToDoApp.Tests.Fixtures.UserFixture;

namespace ToDoApp.Tests.Integration.ToDoItems
{
    public class ToDoItemRepositoryTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IUserRepository _userRepository;

        public ToDoItemRepositoryTests(WebApplicationFactory<Startup> appFactory)
        {
            _scope = appFactory.Services.CreateScope();
            _toDoItemRepository = _scope.ServiceProvider.GetService<IToDoItemRepository>();
            _userRepository = _scope.ServiceProvider.GetService<IUserRepository>();
        }

        [Fact]
        public async Task CanSaveAToDoItem()
        {
            // given:
            var user = CreateAnyUser();
            // and:
            await _userRepository.CreateNew(user);
            // and:
            var toDoItem = CreateToDoItemOwnedBy(user.Id);
            // and:
            await _toDoItemRepository.CreateNew(toDoItem);
            // and:
            toDoItem.Complete();
            // and:
            await _toDoItemRepository.Update(toDoItem);
            // and:
            var toDoItemFromDb = await _toDoItemRepository.FindById(toDoItem.Id);
            // expect:
            toDoItemFromDb.Should().NotBeNull();
            // and:
            toDoItemFromDb.Id.Should().Be(toDoItem.Id);
            toDoItemFromDb.IsOwnedBy(user.Id).Should().BeTrue();
            toDoItemFromDb.IsCompleted.Should().BeTrue();
        }

        public void Dispose() => _scope.Dispose();
    }
}
