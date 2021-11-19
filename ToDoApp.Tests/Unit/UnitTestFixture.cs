using ToDoApp.Domain.ToDoItems;

namespace ToDoApp.Tests.Unit
{
    public class UnitTestFixture
    {
        public ToDoItemService ToDoItemService { get; }

        public UnitTestFixture()
        {
            ToDoItemService = new ToDoItemService(new ToDoItemInMemoryRepository());
        }
    }
}
