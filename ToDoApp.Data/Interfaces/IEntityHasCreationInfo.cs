using System;

namespace ToDoApp.Data.Interfaces
{
    internal interface IEntityHasCreationInfo
    {
        DateTime CreationDate { get; set; }
    }
}
