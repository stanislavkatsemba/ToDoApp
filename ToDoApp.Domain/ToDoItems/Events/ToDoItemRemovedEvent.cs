using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems.Events
{
    public class ToDoItemRemovedEvent : BaseToDoItemEvent
    {
        public ToDoItemRemovedEvent(UserId userId, ToDoItemId toDoItemId) : base(userId, toDoItemId)
        {
        }
    }
}
