using AutoMapper;
using CityVoxWeb.Data.Models.SocialEntities.Enumerators;
using CityVoxWeb.Data.Models.SocialEntities;
using CityVoxWeb.DTOs.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.MappingProfiles
{
    public class SocialProfile : Profile
    {
        public SocialProfile()
        {
            //POSTS
            CreateMap<CreatePostDto, Post>()
                  .ForMember(dest => dest.UserId,
                              opt => opt.MapFrom(src => Guid.Parse(src.UserId)))
                  .ForMember(dest => dest.PostType,
                              opt => opt.MapFrom(src => (PostType)src.PostType))
                  .ForMember(dest => dest.CreatedAt,
                              opt => opt.MapFrom(src => DateTime.UtcNow))
                  .AfterMap((src, dest) =>
                  {
                      var issueId = Guid.Parse(src.IssueId);
                      switch (dest.PostType)
                      {
                          case PostType.Report:
                              dest.ReportId = issueId;
                              break;
                          case PostType.Emergency:
                              dest.EmergencyId = issueId;
                              break;
                          case PostType.Event:
                              dest.EventId = issueId;
                              break;
                          case PostType.InfrastructureIssue:
                              dest.InfrastructureIssueId = issueId;
                              break;
                      }
                  });

            CreateMap<CreateFormalPostDto, Post>()
                  .ForMember(dest => dest.UserId,
                              opt => opt.MapFrom(src => Guid.Parse(src.UserId)))
                  .ForMember(dest => dest.PostType,
                              opt => opt.MapFrom(src => (PostType)4))
                  .ForMember(dest => dest.CreatedAt,
                              opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Post, ExportPostDto>()
                   .ForMember(dest => dest.Id,
                               opt => opt.MapFrom(src => src.Id.ToString()))
                   .ForMember(dest => dest.IssueId,
                               opt => opt.MapFrom(src => GetIssueIdBasedOnPostType(src)))
                   .ForMember(dest => dest.Username,
                               opt => opt.MapFrom(src => src.User.UserName))
                   .ForMember(dest => dest.ProfilePictureUrl,
                               opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                   .ForMember(dest => dest.UpVotesCount,
                               opt => opt.MapFrom(src => src.Votes.Count()))
                   .ForMember(dest => dest.CreatedAt,
                               opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                   .ForMember(dest => dest.PostType,
                               opt => opt.MapFrom(src => src.PostType.ToString()))
                   .ForMember(dest => dest.PostTypeValue,
                               opt => opt.MapFrom(src => (int)src.PostType))
                   .ForMember(dest => dest.IsUpVoted,
                               opt => opt.MapFrom((src, _, _, context) => src.Votes.Any(v => v.UserId == (Guid)context.Items["UserId"])))
                   .ForMember(dest => dest.Comments,
                                opt => opt.MapFrom(src => src.Comments)); // Mapping comments;

            CreateMap<Post, ExportFormalPostDto>()
                   .ForMember(dest => dest.Id,
                               opt => opt.MapFrom(src => src.Id.ToString()))
                   .ForMember(dest => dest.Username,
                               opt => opt.MapFrom(src => src.User.UserName))
                   .ForMember(dest => dest.ProfilePictureUrl,
                               opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                   .ForMember(dest => dest.UpVotesCount,
                               opt => opt.MapFrom(src => src.Votes.Count()))
                   .ForMember(dest => dest.CreatedAt,
                               opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                   .ForMember(dest => dest.IsUpVoted,
                               opt => opt.MapFrom((src, _, _, context) => src.Votes.Any(v => v.UserId == (Guid)context.Items["UserId"])))
                   .ForMember(dest => dest.Comments,
                                opt => opt.MapFrom(src => src.Comments)); // Mapping comments;



            //COMMENTS
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Edited,
                            opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt,
                            opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Comment, ExportCommentDto>()
                .ForMember(dest => dest.CreatedAt,
                            opt => opt.MapFrom(src => src.CreatedAt.ToShortDateString()))
                .ForMember(dest => dest.Username,
                            opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ProfilePictureUrl,
                            opt => opt.MapFrom(src => src.User.ProfilePictureUrl));
        }

        // 1.A function that will handle postType logic
        public string? GetIssueIdBasedOnPostType(Post post)
        {
            switch (post.PostType)
            {
                case PostType.Report:
                    return post.ReportId.ToString();
                case PostType.Emergency:
                    return post.EmergencyId.ToString();
                case PostType.Event:
                    return post.EventId.ToString();
                case PostType.InfrastructureIssue:
                    return post.InfrastructureIssueId.ToString();
                default:
                    return null;
            }
        }
    }
}
