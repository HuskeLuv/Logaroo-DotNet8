using app.DTOs.Post;
using app.Interfaces;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using post.Models;
using post.Controllers;

namespace app.Tests
{
    public class PostControllerTests
    {
        private readonly Mock<IPostRepository> _mockRepo;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockRepo = new Mock<IPostRepository>();
            _controller = new PostController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Test Post 1", Author = "Author 1", Content = "Content 1", PostTags = new List<PostTag>() },
                new Post { Id = 2, Title = "Test Post 2", Author = "Author 2", Content = "Content 2", PostTags = new List<PostTag>() }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync(posts);

            // Act
            var result = await _controller.GetAll(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPosts = Assert.IsType<List<PostDTO>>(okResult.Value);
            Assert.Equal(2, returnPosts.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WithPost()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Test1", Author = "Author1", Content = "Content1" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(post);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PostDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionResult_WithPost()
        {
            // Arrange
            var postDTO = new CreatePostRequestDTO
            {
                Title = "New Post",
                Author = "Test Author",
                Content = "Test Content",
                Tags = new List<string> { "Tag1", "Tag2" }
            };
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" },
                new Tag { Id = 2, Name = "Tag2" }
            };
            var post = new Post
            {
                Id = 1,
                Title = postDTO.Title,
                Author = postDTO.Author,
                Content = postDTO.Content,
                PostTags = tags.Select(tag => new PostTag { Tag = tag }).ToList()
            };

            _mockRepo.Setup(repo => repo.GetAllTagsAsync()).ReturnsAsync(tags);
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(postDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnPost = Assert.IsType<PostDTO>(createdResult.Value);
            Assert.Equal(postDTO.Title, returnPost.Title);
        }

        [Fact]
        public async Task Update_ShouldReturnOkResult_WithUpdatedPost()
        {
            // Arrange
            var updateDTO = new UpdatePostRequestDTO
            {
                Title = "Updated Post",
                Author = "Updated Author",
                Content = "Updated Content",
                Tags = new List<string> { "Tag1", "Tag3" }
            };
            var existingPost = new Post
            {
                Id = 1,
                Title = "Old Post",
                Author = "Old Author",
                Content = "Old Content",
                PostTags = new List<PostTag>()
            };
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" },
                new Tag { Id = 3, Name = "Tag3" }
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPost);
            _mockRepo.Setup(repo => repo.GetAllTagsAsync()).ReturnsAsync(tags);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, updateDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPost = Assert.IsType<PostDTO>(okResult.Value);
            Assert.Equal(updateDTO.Title, returnPost.Title);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContentResult()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Test", Author = "Author", Content = "Content" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(post);
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
