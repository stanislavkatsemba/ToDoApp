﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemSnapshot
    {
        [Key]
        public string Id { get; private set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        public string Name { get; private set; }

        public string Description { get; private set; }

        public DateTime? ScheduledDate { get; private set; }

        public DateTime CreationDate { get; private set; }

        public DateTime? LastEditionDate { get; private set; }

        public bool IsCompleted { get; private set; }

        public DateTime? CompletionData { get; private set; }

        public ToDoItemSnapshot(string id, string userId, string name, string description, DateTime? scheduledDate, DateTime creationDate, DateTime? lastEditionDate, bool isCompleted, DateTime? completionData)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            ScheduledDate = scheduledDate;
            CreationDate = creationDate;
            LastEditionDate = lastEditionDate;
            IsCompleted = isCompleted;
            CompletionData = completionData;
        }
    }
}
