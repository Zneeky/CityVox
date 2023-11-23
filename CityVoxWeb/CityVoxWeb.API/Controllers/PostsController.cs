using CityVoxWeb.DTOs.Social;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly ISocialsService _socialsService;

        public PostsController(ISocialsService socialsService)
        {
            _socialsService = socialsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var wasCreated = await _socialsService.CreatePostAsync(createPostDto);

            return Ok($"Post creation status:{wasCreated}");
        }

        [HttpPost("formal")]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> CreateFormalPost(CreateFormalPostDto createPostDto)
        {
            var wasCreated = await _socialsService.CreateFormalPostAsync(createPostDto);

            return Ok($"Post creation status:{wasCreated}");
        }

        [HttpGet("municipalities/{municipalityId}")]
        public async Task<IActionResult> GetPostsByMunicipality(string municipalityId)
        {
            var userId = User.FindFirstValue("id");
            var posts = await _socialsService.GetPostsByMunicipalityIdAsync(municipalityId, userId);

            return Ok(posts);
        }

        [HttpGet("formal/municipalities/{municipalityId}")]
        public async Task<IActionResult> GetFormalPostsByMunicipality(string municipalityId)
        {
            var userId = User.FindFirstValue("id");
            var posts = await _socialsService.GetFormalPostsByMunicipalityIdAsync(municipalityId, userId);

            return Ok(posts);
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            var wasDeleted = await _socialsService.DeletePostAsync(postId);

            return Ok($"Post delete status:{wasDeleted}");
        }

        [HttpPost("comments")]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            var comments = await _socialsService.CreateCommentAsync(createCommentDto);

            return Ok(comments);
        }

        [HttpPost("vote/{postId}")]
        public async Task<IActionResult> CreateVote(string postId)
        {
            var userId = User.FindFirstValue("id");
            var wasCreated = await _socialsService.CreateVote(postId, userId);

            return Ok($"{wasCreated}");
        }

        [HttpDelete("vote/{postId}")]
        public async Task<IActionResult> DeleteVote(string postId)
        {
            var userId = User.FindFirstValue("id");
            var wasDelted = await _socialsService.DeleteVote(postId, userId);

            return Ok($"{wasDelted}");
        }
    }
}
