using AutoMapper;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using CityVoxWeb.DTOs.Issues.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Mapper.Issue_Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<CreateReportDto, Report>()
                    .ForMember(dest => dest.UserId,
                               opt => opt.MapFrom(src => Guid.Parse(src.CreatorId)))
                    .ForMember(dest => dest.MunicipalityId,
                               opt => opt.MapFrom(src => Guid.Parse(src.MunicipalityId)))
                    .ForMember(dest => dest.ReportTime,
                                opt => opt.MapFrom(src => DateTime.UtcNow))
                    .ForMember(dest => dest.DueBy,
                                opt => opt.MapFrom(src => DateTime.UtcNow.AddMonths(3)))
                    .ForMember(dest => dest.Type,
                                opt => opt.MapFrom(src => (ReportType)src.Type))
                    .ForMember(dest => dest.Status,
                                opt => opt.MapFrom(src => Enum.Parse<ReportStatus>("Reported", true)));

            CreateMap<Report, ExportReportDto>()
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
                                 opt => opt.MapFrom(src => "report"));


            CreateMap<UpdateReportDto, Report>()
                      .ForMember(dest => dest.Type,
                                  opt => opt.MapFrom(src => (ReportType)src.Type))
                       .ForMember(dest => dest.Status,
                                  opt => opt.MapFrom(src => (ReportStatus)src.Status));
        }
    }
}
