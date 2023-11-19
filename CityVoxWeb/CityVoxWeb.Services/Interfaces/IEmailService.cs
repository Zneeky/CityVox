using CityVoxWeb.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendTestEmailAsync(UserEmailOptions userEmailOptions);

        Task SendEmailForEmailConfirmationAsync(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPasswordAsync(UserEmailOptions userEmailOptions);
    }
}
