using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ToDoApp.Web.Common.Authentication
{
    public class JwtService
    {
        private const string SecureKey = "this is a very secure key 7b0f5210-5e9c-412d-a980-bb3defbb5570";

        public string GenerateToken(string userName)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userName),
                new Claim(ClaimTypes.Name, userName),
            };
            var payload = new JwtPayload(userName, audience: null, notBefore: null, claims: claims, expires: DateTime.Now.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public ClaimsPrincipal Verify(string jwt)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));
            var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out _);
            return principal;
        }
    }
}
