using System;

namespace ToDoApp.Domain.Users
{
    public class User
    {
        public UserId Id { get; }

        public string Name { get; }

        public User(UserId userId, string name)
        {
            Name = name;
            Id = userId;
        }

        public static User FromSnapshot(UserSnapshot snapshot)
        {
            return new User(new UserId(new Guid(snapshot.Id)), snapshot.Name);
        }

        public UserSnapshot ToSnapshot()
        {
            return new UserSnapshot(Id.Value.ToString(), Name);
        }
    }
}
