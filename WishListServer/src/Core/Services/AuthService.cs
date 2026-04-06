using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Options;

namespace WishListServer.src.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOpt;
        public AuthService(IOptions<JwtOptions> jwtOpt) =>
            _jwtOpt = jwtOpt.Value;

        public string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOpt.Issuer,
                audience: _jwtOpt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOpt.ExpirationInMinutes),
                signingCredentials: new SigningCredentials(
                    GetSymmetricSecurityKey(_jwtOpt.SecretKey),
                    SecurityAlgorithms.HmacSha256
                )
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}
