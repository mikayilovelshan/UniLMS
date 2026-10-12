using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user);

        string CreateRefreshToken();
    }
}
