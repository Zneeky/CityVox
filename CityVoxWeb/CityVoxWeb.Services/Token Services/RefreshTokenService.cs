using AutoMapper;
using CityVoxWeb.Data.Models;
using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Token_Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly CityVoxDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;

        public RefreshTokenService(CityVoxDbContext context, IConfiguration config, IMapper mapper, IJwtUtils jwtUtils)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public async Task<string> CreateRefreshTokenAsync(UserWithIdDto user)
        {
            // generate a new refresh token for the user and save it in the database
            // return the new refresh token
            _ = int.TryParse(_config["RefreshToken:ValidityInDays"], out int refreshTokenValidityInDays);
            var refreshToken = new RefreshToken
            {
                Token = GetUniqueToken(),
                Expires = DateTime.UtcNow.AddDays(refreshTokenValidityInDays),
                UserId = user.Id,
            };


            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<string> RefreshTokenAsync(string refreshTokenString)
        {
            // validate the provided refresh token and create a new one
            // save the new refresh token in the database and remove the old one
            // return the new refresh token
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshTokenString) ?? throw new NullReferenceException(nameof(refreshTokenString));

            _ = int.TryParse(_config["RefreshToken:ValidityInDays"], out int refreshTokenValidityInDays);
            var refreshToken = new RefreshToken
            {
                Token = GetUniqueToken(),
                Expires = DateTime.UtcNow.AddDays(refreshTokenValidityInDays),
                UserId = oldRefreshToken.UserId,
            };

            _context.RefreshTokens.Remove(oldRefreshToken);
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task RevokeTokenAsync(string refreshTokenString)
        {
            // remove the provided token from the database
            // return true if the operation was successful, false otherwise
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshTokenString) ?? throw new Exception("Refresh token not found");

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        private string GetUniqueToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // ensure token is unique by checking against db
            var tokenIsUnique = !_context.RefreshTokens.Any(rt => rt.Token == token);

            if (!tokenIsUnique)
                return GetUniqueToken();

            return token;
        }

        public async Task<(bool isValid, string userId)> IsValid(string refreshToken, string JwtToken)
        {

            ClaimsPrincipal principal = _jwtUtils.GetPrincipalFromExpiredToken(JwtToken) ?? throw new UnauthorizedAccessException("No principal");

            var idFromToken = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(idFromToken))
            {
                return (false, "");
            }

            var refreshTokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (refreshTokenEntity == null)
            {
                throw new UnauthorizedAccessException("Refresh token malformed or invalid");
            }

            if (refreshTokenEntity.IsExpired)
            {
                return (false, "");
            }

            return (true, idFromToken);
        }
    }
}
