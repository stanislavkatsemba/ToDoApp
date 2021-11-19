using System;
using FluentAssertions;
using ToDoApp.Tests.Fixtures;
using Xunit;

namespace ToDoApp.Tests.Unit.ToDoItems
{
    public class ToDoItemsUpdatingTests
    {
        [Fact]
        public void UserCanUpdateAToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // expect:
            toDoItem.Update("Update item", "New description").IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void UserCanNotUpdateAToDoItemWithAnEmptyName()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // expect:
            toDoItem.Update(string.Empty, string.Empty).IsFailure.Should().BeTrue();
        }
    }
}
