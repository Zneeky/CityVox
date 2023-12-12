using CityVoxWeb.API.Controllers;
using CityVoxWeb.DTOs.Social;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CityVoxWeb.Tests.Controllers
{
    public class PostsControllerTests
    {
        private Mock<ISocialsService> _mockService;
        private PostsController _controller;
        private Mock<ClaimsPrincipal> _mockUser;

        public PostsControllerTests()
        {
            _mockService = new Mock<ISocialsService>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _controller = new PostsController(_mockService.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = _mockUser.Object }
            };
        }

        [Fact]
        public async Task CreatePost_ShouldReturnCorrectStatus()
        {
            // Arrange
            var createPostDto = new CreatePostDto();
            _mockService.Setup(service => service.CreatePostAsync(createPostDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreatePost(createPostDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Post creation status:True", okResult.Value);
        }

        [Fact]
        public async Task GetPostsByMunicipality_ShouldReturnPosts()
        {
            // Arrange
            var userId = "someUserId";
            _mockUser.Setup(user => user.FindFirst(It.IsAny<string>())).Returns(new Claim("id", userId));
            var municipalityId = "someMunicipalityId";
            var expectedPosts = new List<ExportPostDto> { new ExportPostDto() }; // just for the sake of example
            _mockService.Setup(service => service.GetPostsByMunicipalityIdAsync(municipalityId, userId)).ReturnsAsync(expectedPosts);

            // Act
            var result = await _controller.GetPostsByMunicipality(municipalityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedPosts, okResult.Value);
        }

        [Fact]
        public async Task DeletePost_GivenPostId_ShouldReturnDeleteStatus()
        {
            // Arrange
            var postId = "somePostId";
            var expectedDeletionStatus = true;
            _mockService.Setup(service => service.DeletePostAsync(postId)).ReturnsAsync(expectedDeletionStatus);

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Post delete status:{expectedDeletionStatus}", okResult.Value);
        }

        [Fact]
        public async Task CreateComment_GivenValidCommentDto_ShouldReturnCreatedComment()
        {
            // Arrange
            Guid postId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var createCommentDto = new CreateCommentDto
            {
                Text = "someText",
                PostId = postId.ToString(),
                UserId = userId.ToString()
            };

            var expectedComment = new ExportCommentDto
            {
                Id = Guid.NewGuid().ToString(),
                Text = "someText",
                Username = "creatorUsername",
                CreatedAt = DateTime.UtcNow.ToString(),
                Edited = false
            };

            List<ExportCommentDto> exportCommentDtos = new List<ExportCommentDto>
            {
                expectedComment
            };

            _mockService.Setup(service => service.CreateCommentAsync(createCommentDto)).ReturnsAsync(exportCommentDtos);

            // Act
            var result = await _controller.CreateComment(createCommentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
