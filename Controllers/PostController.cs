using app.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace post.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataContext _context;
        public PostController(DataContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public IActionResult GetAll([FromQuery] string? tag = null)
        {
            var posts = _context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(tag)){
                posts = posts.Where(post => post.Tags.Contains(tag));
            }

            return Ok(posts.ToList());
        }

    }
}