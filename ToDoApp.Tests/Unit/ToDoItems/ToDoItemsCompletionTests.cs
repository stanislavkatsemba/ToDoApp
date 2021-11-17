using FluentAssertions;
using ToDoApp.Tests.Fixtures;
using Xunit;

namespace ToDoApp.Tests.Unit.ToDoItems
{
    public class ToDoItemsCompletionTests
    {
        [Fact]
        public void NewlyCreatedToDoItemByDefinitionNotCompleted()
        {
            // expect:
            ToDoItemFixture.CreateAnyToDoItem().IsCompleted.Should().BeFalse();
        }

        [Fact]
        public void UserCanCompleteAToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // expect:
            toDoItem.Complete().IsSuccessful.Should().BeTrue();
            toDoItem.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void UserCanRevokeCompletionAToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // and:
            toDoItem.Complete();
            // expect:
            toDoItem.RevokeCompletion().IsSuccessful.Should().BeTrue();
            toDoItem.IsCompleted.Should().BeFalse();
        }
    }
}
