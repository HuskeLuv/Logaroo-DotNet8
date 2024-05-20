using app.Interfaces;
using Microsoft.EntityFrameworkCore;
using post.Models;
using app.Database;
using app.Models;

namespace app.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        public PostRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetAllAsync(string tag)
        {
            var posts = _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.PostTags.Any(pt => pt.Tag.Name == tag));
            }

            return await posts.ToListAsync();
        }
        public async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}