using AutoMapper;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.Data.Models;
using CityVoxWeb.DTOs.Token;
using CityVoxWeb.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //User Mappings
            CreateMap<RegisterDto, ApplicationUser>();

            CreateMap<LoginDto, ApplicationUser>();

            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<ApplicationUser, UserWithIdDto>()
                    .ForMember(dest => dest.ProfilePicture,
                                opt => opt.MapFrom(src => src.ProfilePictureUrl))
                    .ForMember(dest => dest.Username,
                                opt => opt.MapFrom(src => src.UserName));

            CreateMap<ApplicationUser, UserDefaultDto>()
                    .ForMember(dest => dest.ProfilePicture,
                                opt => opt.MapFrom(src => src.ProfilePictureUrl))
                    .ForMember(dest => dest.Username,
                                opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.SignedUp,
                                opt => opt.MapFrom(src => src.CreatedAt.ToString("d")));


            //MuniRepresntative
            CreateMap<CreateMuniRepDto, MunicipalityRepresentative>()
                .ForMember(dest => dest.MunicipalityId,
                            opt => opt.MapFrom(src => Guid.Parse(src.MunicipalityId)))
                .ForMember(dest => dest.UserId,
                            opt => opt.MapFrom(src => Guid.Parse(src.UserId)))
                .ForMember(dest => dest.StartDate,
                            opt => opt.MapFrom(src => DateTime.UtcNow));


            //Token Mappings
            CreateMap<RefreshToken, RefreshTokenDto>();
        }
    }
}
