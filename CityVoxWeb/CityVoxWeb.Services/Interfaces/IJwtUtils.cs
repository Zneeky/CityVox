using CityVoxWeb.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(UserWithIdDto user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string JwtToken);
    }
}
