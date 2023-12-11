using AutoMapper;
using CityVoxWeb.Data.Models;
using CityVoxWeb.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Mapping_Profiles
{
    public class NotificationProfile : Profile
    {
        NotificationProfile()
        {
            CreateMap<Notification, ExportNotificationDto>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TimeSent,
                           opt => opt.MapFrom(src => src.TimeSent.ToString("d")))
                .ForMember(dest => dest.UserId,
                           opt => opt.MapFrom(src => src.UserId.ToString()));
        }


    }
}
