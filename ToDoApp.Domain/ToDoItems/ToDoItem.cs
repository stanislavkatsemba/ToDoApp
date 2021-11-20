using System;
using ToDoApp.Domain.Shared;
using ToDoApp.Domain.Users;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItem
    {
        public ToDoItemId Id { get; }

        private readonly UserId _userId;

        private string _name;

        private string _description;

        private DateTime? _scheduledDate;

        private bool _isCompleted;

        private DateTime? _completionData;

        public bool IsCompleted => _isCompleted;

        public bool IsOwnedBy(UserId userId) => _userId.Equals(userId);

        public static ToDoItem New(UserId userId, string name, string description, DateTime? scheduledDate = null) =>
            new ToDoItem(ToDoItemId.New(), userId, name, description, scheduledDate, isCompleted: false, completionData: null);

        private ToDoItem(ToDoItemId id, UserId userId, string name, string description, DateTime? scheduledDate,
            bool isCompleted, DateTime? completionData)
        {
            Id = id;
            _userId = userId;
            _name = name;
            _description = description;
            _scheduledDate = scheduledDate;
            _isCompleted = isCompleted;
            _completionData = completionData;
        }

        public Result Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure("Der Aufgabenname darf nicht leer sein.");
            }

            if (_name.Equals(name) && _description.Equals(description))
            {
                return Result.Success();
            }

            _name = name;
            _description = description;
            return Result.Success();
        }

        public Result Complete()
        {
            if (_isCompleted)
            {
                return Result.Success();
            }

            _isCompleted = true;
            _completionData = DateTime.Now;
            return Result.Success();
        }

        public Result RevokeCompletion()
        {
            if (_isCompleted == false)
            {
                return Result.Success();
            }

            _isCompleted = false;
            _completionData = null;
            return Result.Success();
        }

        public Result Schedule(DateTime dateTime)
        {
            if (dateTime < DateTime.Today)
            {
                return Result.Failure("Man kann eine Aufgabe nicht für die Vergangenheit planen.");
            }

            _scheduledDate = dateTime;
            return Result.Success();
        }

        public Result ClearScheduling()
        {
            _scheduledDate = null;
            return Result.Success();
        }

        public static ToDoItem FromSnapshot(ToDoItemSnapshot snapshot)
        {
            return new ToDoItem(new ToDoItemId(new Guid(snapshot.Id)),
                new UserId(new Guid(snapshot.UserId)),
                snapshot.Name,
                snapshot.Description,
                snapshot.ScheduledDate,
                snapshot.IsCompleted,
                snapshot.CompletionData
                );
        }

        public ToDoItemSnapshot ToSnapshot()
        {
            return new ToDoItemSnapshot(Id.Value.ToString(),
                   _userId.Value.ToString(),
                   _name,
                   _description,
                   _scheduledDate,
                   _isCompleted,
                   _completionData);
        }
    }
}
