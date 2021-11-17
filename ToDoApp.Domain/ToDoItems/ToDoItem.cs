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

        public static ToDoItem New(UserId userId, string name, string description, DateTime? scheduledDate = null) =>
            new ToDoItem(ToDoItemId.New(), userId, name, description, scheduledDate, isCompleted: false, completionData: null);

        private ToDoItem(ToDoItemId id, UserId userId, string name, string description, DateTime? scheduledDate, bool isCompleted, DateTime? completionData)
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
            throw new NotImplementedException();
        }

        public Result Complete()
        {
            throw new NotImplementedException();
        }

        public Result RevokeCompletion()
        {
            throw new NotImplementedException();
        }

        public Result Schedule(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Result ClearScheduling()
        {
            throw new NotImplementedException();
        }
    }
}
