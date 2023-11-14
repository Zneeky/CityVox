using CityVoxWeb.DTOs.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface ISocialsService
    {
        Task<bool> CreatePostAsync(CreatePostDto createPostDto);
        Task<bool> CreateFormalPostAsync(CreateFormalPostDto createPostDto);
        Task<bool> DeletePostAsync(string postId);
        Task<ICollection<ExportPostDto>> GetPostsByMunicipalityIdAsync(string municipalityId, string userId);
        Task<ICollection<ExportFormalPostDto>> GetFormalPostsByMunicipalityIdAsync(string municipalityId, string userId);
        Task<ICollection<ExportPostDto>> GetTrendingPosts(string municipalityId);
        Task<ICollection<ExportCommentDto>> CreateCommentAsync(CreateCommentDto createCommentDto);
        Task<ICollection<ExportCommentDto>> DeleteCommentAsync(CreateCommentDto createCommentDto);
        Task<bool> CreateVote(string postId, string userId);
        Task<bool> DeleteVote(string postId, string userId);
    }
}
