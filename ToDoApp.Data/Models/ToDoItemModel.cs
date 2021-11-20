using System;
using ToDoApp.Data.Interfaces;
using ToDoApp.Domain.ToDoItems;

namespace ToDoApp.Data.Models
{
    public class ToDoItemModel : ToDoItemSnapshot, IEntityHasCreationInfo, IEntityHasModificationInfo, IEntitySoftDeletion
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDateTime { get; set; }

        public ToDoItemModel(string id, string userId, string name, string description,
            DateTime? scheduledDate, bool isCompleted, DateTime? completionData) :
            base(id, userId, name, description, scheduledDate, isCompleted, completionData)
        {

        }

        public ToDoItemModel(ToDoItemSnapshot s) : base(s.Id, s.UserId, s.Name, s.Description, s.ScheduledDate, s.IsCompleted, s.CompletionData)
        {

        }
    }
}
