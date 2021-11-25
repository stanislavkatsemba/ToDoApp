using System;
using System.Security.Claims;
using ToDoApp.Domain.Users;

namespace ToDoApp.Web.Common.Authentication
{
    public static class ClaimPrincipalExtension
    {
        public static UserId GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdString, out var userIdGuid) ? new UserId(userIdGuid) : null;
        }
    }
}
