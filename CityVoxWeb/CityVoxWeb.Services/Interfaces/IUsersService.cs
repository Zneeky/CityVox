using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<UserWithIdDto> RegisterUserAsync(RegisterDto registerDto);
        public Task SendEmailConfirmationAsync(ApplicationUser user, string token);
        public Task<UserWithIdDto> AuthenticateUserAsync(LoginDto loginDto);
        public Task<UserWithIdDto> GetUserAsync(string userId);
        public Task<UserDefaultDto> GetByUsernameAsync(string username);
        public Task<UserWithIdDto> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<ICollection<UserDefaultDto>> GetUsersAsync(int page, int count);
        public Task<int> GetUsersCountAsync();
        public Task<bool> CreateNewMuniRepresentativeAsync(CreateMuniRepDto createMuniRepDto);
        public Task<bool> CreateNewAdminAsync(string username);
    }
}
