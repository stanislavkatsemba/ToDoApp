using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ToDoApp.Web.Common.Authentication
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JwtService _jwtService;

        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            JwtService jwtService)
            : base(options, logger, encoder, clock)
        {
            _jwtService = jwtService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey("jwt"))
            {
                // skip authentication if endpoint has [AllowAnonymous] attribute
                var endpoint = Context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }
                // return 401 Unauthorized
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Token"));
            }

            try
            {
                var jwt = Request.Cookies["jwt"];
                var principal = _jwtService.Verify(jwt);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Token"));
            }
        }
    }
}
