using System;

namespace ToDoApp.Domain.ToDoItems.ReadModel
{
    public class ToDoItem
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public DateTime? ScheduledDate { get; private set; }

        public bool IsCompleted { get; private set; }

        public DateTime? CompletionData { get; private set; }

        public DateTime CreationDate { get; private set; }

        public DateTime? ModificationDate { get; private set; }

        public ToDoItem(string id, string name, string description, DateTime? scheduledDate, bool isCompleted, DateTime? completionData, DateTime creationDate, DateTime? modificationDate)
        {
            Id = id;
            Name = name;
            Description = description;
            ScheduledDate = scheduledDate;
            IsCompleted = isCompleted;
            CompletionData = completionData;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }
    }
}
