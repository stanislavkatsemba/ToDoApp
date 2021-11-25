using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems.Events
{
    public class ToDoItemChangedEvent : BaseToDoItemEvent
    {
        public ToDoItemChangedEvent(UserId userId, ToDoItemId toDoItemId) : base(userId, toDoItemId)
        {
        }
    }
}
