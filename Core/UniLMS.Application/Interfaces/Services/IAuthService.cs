using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Auth;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IResult> Register(RegisterDTO model);

        Task<IDataResult<TokenResponseDTO>> Login(LoginDTO model);

        Task<IDataResult<TokenResponseDTO>> RefreshToken(string refreshToken);
    }
}
