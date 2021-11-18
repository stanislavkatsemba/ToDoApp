using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.Users;

namespace ToDoApp.Tests.Fixtures
{
    internal static class ToDoItemFixture
    {
        public static ToDoItem CreateAnyToDoItem() =>
            CreateToDoItemOwnedBy(UserFixture.CreateAnyUser());

        public static ToDoItem CreateToDoItemOwnedBy(UserId userId) =>
            ToDoItem.New(userId, "Create any item", "Any item description");
    }
}
