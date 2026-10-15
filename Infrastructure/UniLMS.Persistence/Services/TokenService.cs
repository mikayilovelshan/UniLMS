using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class TokenService(IConfiguration _configuration) : ITokenService
    {
        public string CreateAccessToken(AppUser user)
        {
            List<Claim> claims = new()
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var secretKey = _configuration["Jwt:SecretKey"];

            var byteKey = Encoding.UTF8.GetBytes(secretKey);

            var symmetricSecurityKey = new SymmetricSecurityKey(byteKey); 

            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _configuration["Jwt: Issuer"],
                audience: _configuration["Jwt: Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt: AccessTokenExpiration"])),
                signingCredentials: signInCredentials
            );
                
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenDescriptor);
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
