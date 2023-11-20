using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _userService; // The user service encapsulates user specific logic.
        private readonly IJwtUtils _jwtUtils; // JWT utility service for security related operations.
        private readonly IRefreshTokenService _refreshTokenService; // Refresh token service for security related operations.
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public AuthController(IUsersService userService, IJwtUtils jwtUtils, IRefreshTokenService refreshTokenService, IConfiguration config, IEmailService emailService)
        {
            _userService = userService;
            _jwtUtils = jwtUtils;
            _refreshTokenService = refreshTokenService;
            _config = config;
            _emailService = emailService;
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
            //var token =

            return Ok("Registered successfully! Please check your email to confirm your account.");
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _userService.ConfirmEmailAsync(uid,token);
                if (result.Succeeded)
                {
                    return Ok("Your email has been confirmed successfully! You can now go and login");
                }
            }
            return BadRequest("Failed to confirm your email!");
        }

        // Login Endpoint
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.AuthenticateUserAsync(loginDto);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid user!" });
            }

            // Generate both tokens
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            // Set both tokens in cookies
            SetTokenInCookie(refreshToken, "refreshToken");
            SetTokenInCookie(jwtToken, "jwtToken");


            return Ok(user);
        }

        // Method to set tokens in cookies
        private void SetTokenInCookie(string token, string cookieName)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(GetTokenValidityDays(cookieName)),
                SameSite = SameSiteMode.None,
                Domain = "localhost", // Adjust as needed
                Secure = true,
            };
            Response.Cookies.Append(cookieName, token, cookieOptions);
        }

        // Helper method to get validity days based on token type
        private int GetTokenValidityDays(string tokenType)
        {
            var validityKey = tokenType == "refreshToken" ? "RefreshToken:ValidityInDays" : "Jwt:TokenValidityInMinutes";
            _ = int.TryParse(_config[validityKey], out int tokenValidityDays);
            return tokenValidityDays;
        }
    }
}
