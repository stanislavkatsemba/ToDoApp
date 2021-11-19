using System;

namespace ToDoApp.Domain.Users
{
    public class UserId : IEquatable<UserId>
    {
        public Guid Value { get; }

        public static UserId New() => new UserId(Guid.NewGuid());

        public UserId(Guid value) => Value = value;

        public bool Equals(UserId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is UserId other && Equals(other);
        
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => $"UserId: {Value}";
    }
}
