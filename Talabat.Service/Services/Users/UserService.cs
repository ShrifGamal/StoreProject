using Microsoft.AspNetCore.Identity;
using Store.Core.DTOs.Auth;
using Store.Core.Entites.Identity;
using Store.Core.ServicesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

      
        public async Task<UserDto> LogInAsync(LogInDto logInDto)
        {
            var user = await _userManager.FindByEmailAsync(logInDto.Email);
            if (user == null) return null;
            var Result = await _signInManager.CheckPasswordSignInAsync(user , logInDto.Password , false);
            if (!Result.Succeeded) return null;

            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DesplayName,
                Token = await _tokenService.CreateTokenAsync(user , _userManager)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExitsAsync(registerDto.Email)) return null;
            var user = new AppUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DesplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user , registerDto.Password);
            if (!result.Succeeded) return null;

            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DesplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<bool> CheckEmailExitsAsync(string email)
        {
           return await _userManager.FindByEmailAsync(email) is not null;
        }

    }
}
