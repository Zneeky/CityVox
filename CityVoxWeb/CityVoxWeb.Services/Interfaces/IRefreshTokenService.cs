using CityVoxWeb.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(UserWithIdDto user);
        Task<string> RefreshTokenAsync(string refreshTokenString);
        Task RevokeTokenAsync(string token);
        Task<(bool isValid, string userId)> IsValid(string refreshToken, string JwtToken);
    }
}
