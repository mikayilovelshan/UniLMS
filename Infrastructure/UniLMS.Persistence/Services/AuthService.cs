using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Auth;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class AuthService(UserManager<AppUser> _userManager,
        ITokenService _tokenService,
        IMapper _mapper) : IAuthService
    {
        public async Task<IDataResult<TokenResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ErrorDataResult<TokenResponseDTO>("Email və ya şifrə yanlışdır");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new ErrorDataResult<TokenResponseDTO>("Email və ya şifrə yanlışdır.");

            var accessToken = _tokenService.CreateAccessToken(user);

            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            var dto = new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return new SuccessDataResult<TokenResponseDTO>(dto);

        }

        public async Task<IDataResult<TokenResponseDTO>> RefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (user == null)
                return new ErrorDataResult<TokenResponseDTO>("İstifadəçi tapılmadı");

            if (user.RefreshTokenEndDate <= DateTime.UtcNow)
                return new ErrorDataResult<TokenResponseDTO>("Yenidən login olmalısız");

            var accessToken = _tokenService.CreateAccessToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            var dto = new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
            };

            return new SuccessDataResult<TokenResponseDTO>(dto);
        }

        public async Task<IResult> Register(RegisterDTO model)
        {
            var email = await _userManager.FindByEmailAsync(model.Email);

            if (email != null)
                return new ErrorResult("Bu emailə malik istifadəçi mövcuddur");

            var user = new AppUser
            {
                FirstName = model.Name,
                LastName = model.Surname,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new ErrorResult("Hesab yaradıla bilmədi");

            return new SuccessResult("Hesab uğurla yaradıldı");
        }
    }
}
