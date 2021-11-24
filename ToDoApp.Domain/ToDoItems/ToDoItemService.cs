using System;
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

        public async Task<Result> Remove(UserId userId, ToDoItemId toDoItemId)
        {
            var toDoItem = await _repository.FindById(toDoItemId);
            var checkResult = Check(userId, toDoItem, toDoItemId);
            if (checkResult.IsFailure)
            {
                return checkResult;
            }
            await _repository.Remove(toDoItem.Id);
            return Result.Success();
        }

        public async Task<Result> Update(UserId userId, ToDoItemId toDoItemId, string name, string description)
        {
            return await ExecuteAction(userId, toDoItemId, toDoItem => toDoItem.Update(name, description));
        }

        public async Task<Result> Complete(UserId userId, ToDoItemId toDoItemId)
        {
            return await ExecuteAction(userId, toDoItemId, toDoItem => toDoItem.Complete());
        }

        public async Task<Result> RevokeCompletion(UserId userId, ToDoItemId toDoItemId)
        {
            return await ExecuteAction(userId, toDoItemId, toDoItem => toDoItem.RevokeCompletion());
        }

        public async Task<Result> Schedule(UserId userId, ToDoItemId toDoItemId, DateTime date)
        {
            return await ExecuteAction(userId, toDoItemId, toDoItem => toDoItem.Schedule(date));
        }

        public async Task<Result> ClearScheduling(UserId userId, ToDoItemId toDoItemId, DateTime date)
        {
            return await ExecuteAction(userId, toDoItemId, toDoItem => toDoItem.ClearScheduling());
        }

        private async Task<Result> ExecuteAction(UserId userId, ToDoItemId toDoItemId, Func<ToDoItem, Result> action)
        {
            var toDoItem = await _repository.FindById(toDoItemId);
            var checkResult = Check(userId, toDoItem, toDoItemId);
            if (checkResult.IsFailure)
            {
                return checkResult;
            }

            var result = action(toDoItem);
            if (result.IsSuccessful)
            {
                await _repository.Update(toDoItem);
            }

            return result;
        }

        private static Result Check(UserId userId, ToDoItem toDoItem, ToDoItemId toDoItemId)
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
