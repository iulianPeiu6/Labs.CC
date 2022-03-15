using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UScheduler.WebApi.Gateway.Interfaces;
using UScheduler.WebApi.Gateway.Options;

namespace UScheduler.WebApi.Gateway.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationConfiguration config;

        public AuthenticationService(IOptions<AuthenticationConfiguration> config)
        {
            this.config = config.Value;
        }

        public string GenerateToken(Guid id, string username, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.NameId, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };


            var token = new JwtSecurityToken(config.Key,
              config.Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(config.MinutesExpiration),
              signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
