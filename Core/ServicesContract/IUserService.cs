using Store.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ServicesContract
{
    public interface IUserService
    {
        Task<UserDto> LogInAsync(LogInDto logInDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckEmailExitsAsync(string email);
    }
}
