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

        private DateTime? _creationDate;

        private DateTime? _lastEditionDate;

        private bool _isCompleted;

        private DateTime? _completionData;

        public bool IsCompleted => _isCompleted;

        public bool IsOwnedBy(UserId userId) => _userId.Equals(userId);

        public static ToDoItem New(UserId userId, string name, string description, DateTime? scheduledDate = null) =>
            new ToDoItem(ToDoItemId.New(), userId, name, description, scheduledDate, isCompleted: false, completionData: null, creationDate: DateTime.Now, lastEditionDate: null);

        private ToDoItem(ToDoItemId id, UserId userId, string name, string description, DateTime? scheduledDate,
            bool isCompleted, DateTime? completionData, DateTime? creationDate, DateTime? lastEditionDate)
        {
            Id = id;
            _userId = userId;
            _name = name;
            _description = description;
            _scheduledDate = scheduledDate;
            _isCompleted = isCompleted;
            _completionData = completionData;
            _creationDate = creationDate;
            _lastEditionDate = lastEditionDate;
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
            UpdateLastEditionDate();
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
            UpdateLastEditionDate();
            return Result.Success();
        }

        public Result ClearScheduling()
        {
            _scheduledDate = null;
            UpdateLastEditionDate();
            return Result.Success();
        }

        private void UpdateLastEditionDate()
        {
            _lastEditionDate = DateTime.Now;
        }
    }
}
