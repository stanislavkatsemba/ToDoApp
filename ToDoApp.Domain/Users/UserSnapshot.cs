using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Users
{
    public class UserSnapshot
    {
        [Key]
        public string Id { get; private set; }

        [Required]
        public string Name { get; private set; }

        public UserSnapshot(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
