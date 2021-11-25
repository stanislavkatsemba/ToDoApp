using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Domain.Shared;
using ToDoApp.Domain.Users;
using ToDoApp.Web.Common.Authentication;

namespace ToDoApp.Web.WebAPI.Authentication
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<Result> Register([FromBody]string userName)
        {
            return await _userService.Register(userName);
        }

        [HttpPost("authentication")]
        public async Task<Result> Login([FromBody]string userName)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Result.Success();
            }

            var result = await _userService.FindByName(userName);
            if (result == null)
            {
                return Result.Failure("Falsches Benutzernamen oder Kennwort.");
            }

            var jwt = _jwtService.GenerateToken(userName, result.Id.Value.ToString());
            Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true });
            
            return Result.Success();
        }

        [HttpGet("user")]
        public async Task<UserInfo> IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.FindByName(User.Identity.Name);
                return new UserInfo(user.Name);
            }
            return null;
        }

        [HttpPost("logout")]
        public Task<Result> Logout()
        {
            Response.Cookies.Delete("jwt");
            return Task.FromResult(Result.Success());
        }
    }
}
