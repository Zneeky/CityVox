using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _userService; // The user service encapsulates user specific logic.
        private readonly IJwtUtils _jwtUtils; // JWT utility service for security related operations.
        private readonly IRefreshTokenService _refreshTokenService; // Refresh token service for security related operations.
        private readonly IConfiguration _config;
        public AuthController(IUsersService userService, IJwtUtils jwtUtils, IRefreshTokenService refreshTokenService, IConfiguration config)
        {
            _userService = userService;
            _jwtUtils = jwtUtils;
            _refreshTokenService = refreshTokenService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _userService.RegisterUserAsync(registerDto);

            if (user == null)
            {
                return BadRequest(new { message = "User already exists!" });
            }


            return Ok("Registered successfully! Please check your email to confirm your account.");
        }
    }
}
