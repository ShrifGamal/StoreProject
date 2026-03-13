using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.APIs.Extensions;
using Store.Core.DTOs.Auth;
using Store.Core.Entites.Identity;
using Store.Core.ServicesContract;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.APIs.Controllers
{
   
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountsController(
            IUserService userService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            ITokenService tokenService
            
            )
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserDto>> LogIn (LogInDto logInDto)
        {
            var user = await _userService.LogInAsync(logInDto);
            if(user is null) return  Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Redister (RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if(user is null) return  BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest , "Invalid Registration"));
            return Ok(user);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var user  = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DesplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

        }

        [HttpGet("GetCurrentAddress")]
        public async Task<ActionResult<UserDto>> GetCurrentAddress()
        {
           
            var user  = await _userManager.FindByEmailWithAddressAsync(User);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
           
        }


    }
}
