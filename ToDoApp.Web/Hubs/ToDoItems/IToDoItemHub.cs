using System.Threading.Tasks;
using ToDoApp.Domain.ToDoItems.ReadModel;

namespace ToDoApp.Web.Hubs.ToDoItems
{
    public interface IToDoItemHub
    {
        Task ReceiveToDoItem(ToDoItem message);
        Task ToDoItemRemoved(string toDoItemId);
    }
}