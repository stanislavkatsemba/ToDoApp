using System;
using FluentAssertions;
using ToDoApp.Tests.Fixtures;
using Xunit;

namespace ToDoApp.Tests.Unit.ToDoItems
{
    public class ToDoItemsSchedulingTests
    {
        [Fact]
        public void UserCanScheduleAToDoItem()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // expect:
            toDoItem.Schedule(DateTime.Today).IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void UserCanNotScheduleAToDoItemForThePast()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // expect:
            toDoItem.Schedule(DateTime.Today.AddDays(-1)).IsFailure.Should().BeTrue();
        }

        [Fact]
        public void UserCanClearAToDoItemScheduling()
        {
            // given:
            var toDoItem = ToDoItemFixture.CreateAnyToDoItem();
            // and:
            toDoItem.Schedule(DateTime.Today);
            //expect:
            toDoItem.ClearScheduling().IsSuccessful.Should().BeTrue();
        }
    }
}
