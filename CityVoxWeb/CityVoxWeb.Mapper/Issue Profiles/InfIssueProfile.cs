using AutoMapper;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.InfIssue;
using CityVoxWeb.DTOs.Issues.InfIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.AutoMapper
{
    public class InfIssueProfile : Profile
    {
        public InfIssueProfile()
        {
            CreateMap<CreateInfIssueDto, InfrastructureIssue>()
                     .ForMember(dest => dest.UserId,
                                opt => opt.MapFrom(src => Guid.Parse(src.CreatorId)))
                     .ForMember(dest => dest.MunicipalityId,
                                opt => opt.MapFrom(src => Guid.Parse(src.MunicipalityId)))
                     .ForMember(dest => dest.ReportTime,
                                 opt => opt.MapFrom(src => DateTime.UtcNow))
                     .ForMember(dest => dest.DueBy,
                                opt => opt.MapFrom(src => DateTime.UtcNow.AddMonths(6)))
                     .ForMember(dest => dest.Type,
                                 opt => opt.MapFrom(src => (InfrastructureIssueType)src.Type))
                     .ForMember(dest => dest.Status,
                                 opt => opt.MapFrom(src => Enum.Parse<InfrastructureIssueStatus>("Reported", true)));

            CreateMap<InfrastructureIssue, ExportInfIssueDto>()
                     .ForMember(dest => dest.ReportTime,
                                 opt => opt.MapFrom(src => src.ReportTime.ToString("MM/dd/yyyy")))
                     .ForMember(dest => dest.ResolvedTime,
                                 opt => opt.MapFrom(src => src.ResolvedTime.GetValueOrDefault().ToString("dd/mm/yy")))
                     .ForMember(dest => dest.Id,
                                 opt => opt.MapFrom(src => src.Id.ToString()))
                     .ForMember(dest => dest.CreatorUsername,
                                 opt => opt.MapFrom(src => src.User.UserName))
                     .ForMember(dest => dest.Municipality,
                                 opt => opt.MapFrom(src => src.Municipality.MunicipalityName))
                     .ForMember(dest => dest.Type,
                                 opt => opt.MapFrom(src => src.Type.ToString()))
                     .ForMember(dest => dest.Status,
                                 opt => opt.MapFrom(src => src.Status.ToString()))
                     .ForMember(dest => dest.TypeValue,
                                 opt => opt.MapFrom(src => (int)src.Type))
                     .ForMember(dest => dest.StatusValue,
                                 opt => opt.MapFrom(src => (int)src.Status))
                     .ForMember(dest => dest.Represent,
                                 opt => opt.MapFrom(src => "infrastructure_issue"));


            CreateMap<UpdateInfIssueDto, InfrastructureIssue>()
                      .ForMember(dest => dest.Type,
                                  opt => opt.MapFrom(src => (InfrastructureIssueType)src.Type))
                       .ForMember(dest => dest.Status,
                                  opt => opt.MapFrom(src => (InfrastructureIssueStatus)src.Status));
        }
    }
}
