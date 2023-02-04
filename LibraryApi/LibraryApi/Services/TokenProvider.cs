using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryApi.DTO;
using LibraryApi.Enums;
using LibraryApi.ExtensionMethods;
using LibraryApi.Services.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _configuration;

        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenData Create(int userId, UserRole userRole)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var expirationInMinutes = jwtConfig.GetValue<int>("ExpirationInMinutes");
            var secret = jwtConfig.GetValue<string>("Secret");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, userRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = new TokenData
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationTimestamp = tokenDescriptor.Expires.ToTimestamp().Value
            };
            return response;
        }
    }
}