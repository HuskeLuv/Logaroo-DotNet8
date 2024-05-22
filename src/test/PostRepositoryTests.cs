using app.Database;
using app.Repository;
using Microsoft.EntityFrameworkCore;
using post.Models;

namespace app.Tests
{
    public class PostRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public PostRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "PostDatabase")
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPosts()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Posts.Add(new Post { Title = "Test1", Author = "Author1", Content = "Content1" });
                context.Posts.Add(new Post { Title = "Test2", Author = "Author2", Content = "Content2" });
                context.SaveChanges();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new PostRepository(context);
                var result = await repository.GetAllAsync(null);

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectPost()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Posts.Add(new Post { Id = 1, Title = "Test1", Author = "Author1", Content = "Content1" });
                context.SaveChanges();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new PostRepository(context);
                var result = await repository.GetByIdAsync(1);

                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task CreateAsync_ShouldAddPost()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                var repository = new PostRepository(context);
                var post = new Post { Title = "Test", Author = "Author", Content = "Content" };

                await repository.CreateAsync(post);
                var result = await context.Posts.FirstOrDefaultAsync(p => p.Title == "Test");

                Assert.NotNull(result);
                Assert.Equal("Test", result.Title);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePost()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                var post = new Post { Id = 1, Title = "Test", Author = "Author", Content = "Content" };
                context.Posts.Add(post);
                context.SaveChanges();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new PostRepository(context);

                await repository.DeleteAsync(1);
                var result = await context.Posts.FindAsync(1);

                Assert.Null(result);
            }
        }
    }
}
