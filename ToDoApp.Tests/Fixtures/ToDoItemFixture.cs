using ToDoApp.Domain.ToDoItems;

namespace ToDoApp.Tests.Fixtures
{
    internal static class ToDoItemFixture
    {
        public static ToDoItem CreateAnyToDoItem() => 
            ToDoItem.New(UserFixture.CreateAnyUser(), "Create any item", "Any item description");
    }
}
