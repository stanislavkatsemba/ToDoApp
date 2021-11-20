using System;
using ToDoApp.Domain.Users;

namespace ToDoApp.Tests.Fixtures
{
    public static class UserFixture
    {
        public static UserId CreateAnyUserId() => new UserId(Guid.NewGuid());

        public static User CreateAnyUser() => new User(CreateAnyUserId(), "Test user " + Guid.NewGuid());
    }
}
