using System;

namespace ToDoApp.Domain.ToDoItems
{
    public class ToDoItemId : IEquatable<ToDoItemId>
    {
        public Guid Value { get; }

        public static ToDoItemId New() => new ToDoItemId(Guid.NewGuid());

        public ToDoItemId(Guid value) => Value = value;

        public bool Equals(ToDoItemId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is ToDoItemId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => $"{nameof(ToDoItemId)}: {Value}";
    }
}
