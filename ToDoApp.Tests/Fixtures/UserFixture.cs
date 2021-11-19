using System;
using ToDoApp.Domain.Users;

namespace ToDoApp.Tests.Fixtures
{
    public static class UserFixture
    {
        public static UserId CreateAnyUser() => new UserId(Guid.NewGuid());
    }
}
