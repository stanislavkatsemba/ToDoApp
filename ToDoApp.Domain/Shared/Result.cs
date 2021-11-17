using System;

namespace ToDoApp.Domain.Shared
{
    public readonly struct Result : IEquatable<Result>
    {
        public bool IsSuccessful { get; }
        
        public bool IsFailure => !IsSuccessful;

        public string Reason { get; }

        public static Result Success() => new Result(true, "Ok");

        public static Result Failure(string reason) => new Result(false, reason);

        private Result(bool isSuccessful, string reason)
        {
            IsSuccessful = isSuccessful;
            Reason = reason;
        }

        public bool Equals(Result other) => (IsSuccessful, Reason).Equals((other.IsSuccessful, other.Reason));

        public override bool Equals(object obj) => obj is Result other && Equals(other);
        
        public override int GetHashCode() => (IsSuccessful, Reason).GetHashCode();
    }
}
