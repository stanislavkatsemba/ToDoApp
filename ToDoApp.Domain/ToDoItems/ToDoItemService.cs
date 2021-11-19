using System.Threading.Tasks;
using ToDoApp.Domain.Shared;
using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemService
    {
        private readonly IToDoItemRepository _repository;

        public ToDoItemService(IToDoItemRepository repository) =>
            _repository = repository;

        public async Task<Result> Create(ToDoItem toDoItem)
        {
            await _repository.CreateNew(toDoItem);
            return Result.Success();
        }

        public async Task<Result> Update(UserId userId, ToDoItemId toDoItemId, string name, string description)
        {
            var toDoItem = await _repository.FindBy(toDoItemId);
            var checkResult = Check(userId, toDoItem, toDoItemId);
            if (checkResult.IsFailure)
            {
                return checkResult;
            }

            var result = toDoItem.Update(name, description);
            await _repository.Update(toDoItem);
            return result;
        }

        public async Task<Result> Complete(UserId userId, ToDoItemId toDoItemId)
        {
            var toDoItem = await _repository.FindBy(toDoItemId);
            var checkResult = Check(userId, toDoItem, toDoItemId);
            if (checkResult.IsFailure)
            {
                return checkResult;
            }

            var result = toDoItem.Complete();
            await _repository.Update(toDoItem);
            return result;
        }

        private Result Check(UserId userId, ToDoItem toDoItem, ToDoItemId toDoItemId)
        {
            if (toDoItem == null)
            {
                return Result.Failure($"Die Aufgabe mit der ID {toDoItemId.Value} ist nicht vorhanden.");
            }

            if (!toDoItem.IsOwnedBy(userId))
            {
                return Result.Failure($"Die Aufgabe mit der id {toDoItemId.Value} gehört nicht dem Benutzer {userId.Value}");
            }

            return Result.Success();
        }
    }
}
