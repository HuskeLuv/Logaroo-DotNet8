using app.Database;
using app.DTOs.Post;
using app.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var posts = _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .AsQueryable();
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.PostTags.Any(pt => pt.Tag.Name == tag));
            }

            var postDTOs = posts.ToList().Select(p => p.ToPostDTO()).ToList();

            return Ok(postDTOs);;
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetById([FromRoute] int id){
            var post = _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.Id == id);
            if (post == null){
                return NotFound();
            }
            return Ok(post.ToPostDTO());
        }

        [HttpPost, Authorize]
        public IActionResult Create([FromBody] CreatePostRequestDTO postDTO)
        {
            var existingTags = _context.Tags.ToList();
            var postModel = postDTO.ToPostFromCreateDTO(existingTags);
            _context.Posts.Add(postModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, postModel.ToPostDTO());
        }
    }
}