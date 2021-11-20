using System;
using ToDoApp.Data.Interfaces;
using ToDoApp.Domain.Users;

namespace ToDoApp.Data.Models
{
    public class UserModel : UserSnapshot, IEntityHasCreationInfo
    {
        public UserModel(string id, string name) : base(id, name)
        {
        }

        public UserModel(UserSnapshot s) : base(s.Id, s.Name)
        {
        }

        public DateTime CreationDate { get; set; }
    }
}
