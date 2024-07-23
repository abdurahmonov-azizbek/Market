using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Market.Application.Services
{
    public class TokenService(IOptions<JwtSettings> jwtSettingsOptions) : ITokenService
    {
        private JwtSettings jwtSettings = jwtSettingsOptions.Value;

        public string GetToken(User user)
        {
            var claims = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpirationTimeInMinutes),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        private List<Claim> GetClaims(User user)
            => new List<Claim>
            {
                new Claim(nameof(User.Id), user.Id.ToString()),
                new Claim(nameof(User.FirstName), user.FirstName),
                new Claim(nameof(User.LastName), user.LastName),
                new Claim(nameof(User.Email), user.Email),
                new Claim(nameof(User.Role), user.Role.ToString())
            };
    }
}
