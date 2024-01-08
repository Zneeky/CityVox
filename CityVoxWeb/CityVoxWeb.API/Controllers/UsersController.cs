using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly IJwtUtils _jwtUtils;
        public UsersController(IUsersService userService, IJwtUtils jwtUtils)
        {
            _userService = userService;
            _jwtUtils = jwtUtils;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers(int page, int count)
        {
            var users = await _userService.GetUsersAsync(page, count);

            return Ok(users);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersCount()
        {
            var usersCount = await _userService.GetUsersCountAsync();

            return Ok(usersCount);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            return Ok(user);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUsers([FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userService.UpdateUserAsync(updateUserDto);

            return Ok(user);
        }

        [HttpPost("admins/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(string username)
        {
            var wasCreated = await _userService.CreateNewAdminAsync(username);

            return Ok(wasCreated);
        }

        [HttpPost("representatives")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMuniRepresentative([FromBody] CreateMuniRepDto createMuniRepDto)
        {
            var wasCreated = await _userService.CreateNewMuniRepresentativeAsync(createMuniRepDto);

            return Ok(wasCreated);
        }
    }
}
