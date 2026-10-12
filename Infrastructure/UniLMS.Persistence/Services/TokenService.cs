using Microsoft.Extensions.Configuration;
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

            throw new NotImplementedException();
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
