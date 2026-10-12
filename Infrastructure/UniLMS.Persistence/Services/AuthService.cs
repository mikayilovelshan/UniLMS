using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Auth;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class AuthService(UserManager<AppUser> _userManager) : IAuthService
    {
        public async Task<IDataResult<TokenResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ErrorDataResult<TokenResponseDTO>("Email və ya şifrə yanlışdır");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new ErrorDataResult<TokenResponseDTO>("Email və ya şifrə yanlışdır.");

            
        }

        public Task<IDataResult<TokenResponseDTO>> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
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
