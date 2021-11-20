using System;

namespace ToDoApp.Data.Interfaces
{
    internal interface IEntitySoftDeletion
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletionDateTime { get; set; }
    }
}
