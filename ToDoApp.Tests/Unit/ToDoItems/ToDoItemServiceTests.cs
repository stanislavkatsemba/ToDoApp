using System.Threading.Tasks;
using FluentAssertions;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.Users;
using ToDoApp.Tests.Fixtures;
using Xunit;

namespace ToDoApp.Tests.Unit.ToDoItems
{
    public class ToDoItemServiceTests
    {
        private readonly UserId _userId = UserFixture.CreateAnyUserId();
        private readonly ToDoItemService _toDoItemService;

        public ToDoItemServiceTests()
        {
            var fixture = new UnitTestFixture();
            _toDoItemService = fixture.ToDoItemService;
        }

        [Fact]
        public async Task CannotCompleteANotExistingToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateToDoItemOwnedBy(_userId);
            // when:
            var result = await _toDoItemService.Complete(_userId, toDoItem.Id);
            // then:
            result.IsFailure.Should().BeTrue();
            result.Reason.Should().Contain("ist nicht vorhanden");
        }

        [Fact]
        public async Task CanCompleteToAnExistingToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateToDoItemOwnedBy(_userId);
            // and:
            await _toDoItemService.Create(toDoItem);
            // when:
            var result = await _toDoItemService.Complete(_userId, toDoItem.Id);
            // then:
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task CannotCompleteAToDoItemWhichIsNotOwnedByAnUser()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateToDoItemOwnedBy(_userId);
            // and:
            await _toDoItemService.Create(toDoItem);
            // when:
            var result = await _toDoItemService.Complete(UserFixture.CreateAnyUserId(), toDoItem.Id);
            // then:
            result.IsFailure.Should().BeTrue();
        }
    }
}
