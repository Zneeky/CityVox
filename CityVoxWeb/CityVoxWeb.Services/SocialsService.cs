using AutoMapper;
using CityVoxWeb.Data.Models.SocialEntities;
using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Social;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CityVoxWeb.Services
{
    public class SocialsService : ISocialsService
    {
        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;
        public SocialsService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreatePostAsync(CreatePostDto createPostDto)
        {
            try
            {
                var post = _mapper.Map<Post>(createPostDto);
                await _dbContext.Posts.AddAsync(post);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create a new post!", ex);
            }

        }

        public async Task<bool> CreateFormalPostAsync(CreateFormalPostDto createPostDto)
        {
            try
            {
                var post = _mapper.Map<Post>(createPostDto);
                await _dbContext.Posts.AddAsync(post);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create a new formal post!", ex);
            }
        }
        public async Task<ICollection<ExportCommentDto>> CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            try
            {
                var comment = _mapper.Map<Comment>(createCommentDto);
                await _dbContext.Comments.AddAsync(comment);
                await _dbContext.SaveChangesAsync();
                var comments = await _dbContext.Comments
                    .Include(c => c.User)
                    .Where(c => c.PostId.ToString() == createCommentDto.PostId)
                    .ToListAsync();
                var commentsDtos = _mapper.Map<List<ExportCommentDto>>(comments);
                return commentsDtos;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create a new comment!", ex);
            }
        }

        public Task<ICollection<ExportCommentDto>> DeleteCommentAsync(CreateCommentDto createCommentDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePostAsync(string postId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ExportPostDto>> GetPostsByMunicipalityIdAsync(string municipalityId, string userId)
        {
            try
            {
                var representativeUserIds = await _dbContext.MunicipalityRepresentatives
                    .Where(mr => mr.MunicipalityId.ToString() == municipalityId)
                    .Select(mr => mr.UserId)
                    .ToListAsync();

                var posts = await _dbContext.Posts
                    .Include(p => p.User)
                    .Include(p => p.Votes)
                    .Include(p => p.Report)
                    .Include(p => p.Emergency)
                    .Include(p => p.Event)
                    .Include(p => p.InfrastructureIssue)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .Where(p => p.Report != null && p.Report.MunicipalityId.ToString() == municipalityId
                             || p.Emergency != null && p.Emergency.MunicipalityId.ToString() == municipalityId
                             || p.Event != null && p.Event.MunicipalityId.ToString() == municipalityId
                             || p.InfrastructureIssue != null && p.InfrastructureIssue.MunicipalityId.ToString() == municipalityId)
                    .Where(p => !representativeUserIds.Contains(p.UserId))
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                var dtoPosts = _mapper.Map<ICollection<ExportPostDto>>(posts, opt => opt.Items["UserId"] = Guid.Parse(userId));

                return dtoPosts;
            }
            catch (Exception ex)
            {
                // Handle exception here
                throw new Exception("Could not get posts!", ex);
            }
        }


        public async Task<ICollection<ExportFormalPostDto>> GetFormalPostsByMunicipalityIdAsync(string municipalityId, string userId)
        {
            try
            {
                // Get the IDs of the representatives from the provided municipality
                var representativeUserIds = await _dbContext.MunicipalityRepresentatives
                    .Where(mr => mr.MunicipalityId.ToString() == municipalityId)
                    .Select(mr => mr.UserId)
                    .ToListAsync();

                // Fetch the posts from those representatives
                var posts = await _dbContext.Posts
                    .Include(p => p.User)
                    .Include(p => p.Votes)
                    .Include(p => p.Report)
                    .Include(p => p.Emergency)
                    .Include(p => p.Event)
                    .Include(p => p.InfrastructureIssue)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .Where(p => representativeUserIds.Contains(p.UserId))
                    .ToListAsync();

                // Map the posts to the DTO and return them
                var dtoPosts = _mapper.Map<ICollection<ExportFormalPostDto>>(posts, opt => opt.Items["UserId"] = Guid.Parse(userId));

                return dtoPosts;
            }
            catch (Exception ex)
            {
                // Handle exception here
                throw new Exception("Could not get formal posts!", ex);
            }
        }

        public Task<ICollection<ExportPostDto>> GetTrendingPosts(string municipalityId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateVote(string postId, string userId)
        {

            try
            {
                VotePost vote = new VotePost
                {
                    PostId = Guid.Parse(postId),
                    UserId = Guid.Parse(userId),
                    CreatedAt = DateTime.UtcNow,
                    IsUpvote = true,
                    Id = Guid.NewGuid(),
                };

                await _dbContext.PostsVotes.AddAsync(vote);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create a new vote!", ex);
            }
        }

        public async Task<bool> DeleteVote(string postId, string userId)
        {
            try
            {
                VotePost vote = await _dbContext.PostsVotes.FirstOrDefaultAsync(vp => vp.PostId.ToString() == postId && vp.UserId.ToString() == userId) ?? throw new ArgumentNullException("Such vote was not found");
                _dbContext.PostsVotes.Remove(vote);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not delete the vote!", ex);
            }
        }
    }
}
