using AutoMapper;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CityVoxWeb.DTOs.User;
using Microsoft.Extensions.Configuration;

namespace CityVoxWeb.Services.User_Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly CityVoxDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsersService(IMapper mapper, CityVoxDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration, IEmailService emailService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
           _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserWithIdDto> RegisterUserAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return null;
            }

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.CreatedAt = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                string errorMessage = "";
                foreach (var error in result.Errors)
                {
                    errorMessage += $" {error.Description};";
                }
                throw new Exception($"{errorMessage}");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if(!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationAsync(user, token);
            }

            await _userManager.AddToRoleAsync(user, "user");
            var userDto = _mapper.Map<UserWithIdDto>(user);
            userDto.Role = "user";

            return userDto;
        }

        public async Task SendEmailConfirmationAsync(ApplicationUser user, string token)
        {
            try
            {
                string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
                };

                await _emailService.SendEmailForEmailConfirmationAsync(options);
            }
            catch ( Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task<UserWithIdDto> AuthenticateUserAsync(LoginDto loginDto)
        {
            // Find the user
            var user = await _userManager.FindByEmailAsync(loginDto.Email);


            // If user does not exist or password is not correct, return null
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password) || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var sortedRoles = roles.OrderBy(r => r).ToList();

            // User authenticated successfully, map to UserWithIdDto
            var userDto = _mapper.Map<UserWithIdDto>(user);
            userDto.Role = sortedRoles.First();
            return userDto;
        }

        public async Task<UserWithIdDto> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var sortedRoles = roles.OrderBy(r => r).ToList();

            var userDto = _mapper.Map<UserWithIdDto>(user);
            userDto.Role = sortedRoles.First();

            return userDto;
        }

        public async Task<UserWithIdDto> GetByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            var sortedRoles = roles.OrderBy(r => r).ToList();
            var userDto = _mapper.Map<UserWithIdDto>(user);
            userDto.Role = sortedRoles.First();

            return userDto;
        }

        public async Task<UserWithIdDto> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            try
            {
                var originalUser = await _userManager.FindByNameAsync(updateUserDto.Username);
                _mapper.Map(updateUserDto, originalUser);
                await _userManager.UpdateAsync(originalUser);

                var roles = await _userManager.GetRolesAsync(originalUser);
                var sortedRoles = roles.OrderBy(r => r).ToList();

                var userDto = _mapper.Map<UserWithIdDto>(originalUser);
                userDto.Role = sortedRoles.First();

                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update the user", ex);
            }
        }

        public async Task<ICollection<UserDefaultDto>> GetUsersAsync(int page, int count)
        {
            try
            {
                var users = await _dbContext.Users
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                var usersDto = new List<UserDefaultDto>();

                foreach (var user in users)
                {
                    var dto = _mapper.Map<UserDefaultDto>(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    dto.Role = roles.OrderBy(r => r).First();
                    usersDto.Add(dto);
                }

                return usersDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<int> GetUsersCountAsync()
        {
            try
            {
                var usersCount = await _dbContext.Users.CountAsync();
                return usersCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<bool> CreateNewMuniRepresentativeAsync(CreateMuniRepDto createMuniRepDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(createMuniRepDto.UserName) ?? throw new ArgumentNullException("There is no such user!");
                createMuniRepDto.UserId = user.Id.ToString();

                var muniRep = _mapper.Map<MunicipalityRepresentative>(createMuniRepDto);

                _dbContext.MunicipalityRepresentatives.Add(muniRep);
                await _dbContext.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "representative");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<bool> CreateNewAdminAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentNullException("There is no such user!");
                await _userManager.AddToRoleAsync(user, "admin");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        
    }
}
