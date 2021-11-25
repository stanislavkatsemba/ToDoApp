using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.ToDoItems.ReadModel;
using ToDoApp.Domain.Users;
using ToDoApp.Web;
using Xunit;
using static ToDoApp.Tests.Fixtures.ToDoItemFixture;
using static ToDoApp.Tests.Fixtures.UserFixture;

namespace ToDoApp.Tests.Integration.ToDoItems
{
    public class ToDoItemReadRepositoryTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IToDoItemReadRepository _toDoItemReadRepository;
        private readonly IUserRepository _userRepository;

        public ToDoItemReadRepositoryTests(WebApplicationFactory<Startup> appFactory)
        {
            _scope = appFactory.Services.CreateScope();
            _toDoItemRepository = _scope.ServiceProvider.GetService<IToDoItemRepository>();
            _userRepository = _scope.ServiceProvider.GetService<IUserRepository>();
            _toDoItemReadRepository = _scope.ServiceProvider.GetService<IToDoItemReadRepository>();
        }

        [Fact]
        public async Task CanReadAToDoItem()
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
            var toDoItemFromDb = await _toDoItemReadRepository.GetById(user.Id, toDoItem.Id);
            // expect:
            toDoItemFromDb.Should().NotBeNull();
            // and:
            var toDoItemsFromDb = await _toDoItemReadRepository.GetAllForUser(user.Id);
            // expect:
            toDoItemsFromDb.Count().Should().BeGreaterThan(0);
        }

        public void Dispose() => _scope.Dispose();
    }
}
