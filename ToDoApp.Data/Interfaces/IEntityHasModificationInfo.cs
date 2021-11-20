using System;

namespace ToDoApp.Data.Interfaces
{
    internal interface IEntityHasModificationInfo
    {
        DateTime? ModificationDate { get; set;}
    }
}
