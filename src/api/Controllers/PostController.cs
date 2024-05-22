using app.DTOs.Post;
using app.Interfaces;
using app.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace post.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? tag = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var posts = await _postRepository.GetAllAsync(tag);
            var postDTOs = posts.Select(p => p.ToPostDTO()).ToList();
            return Ok(postDTOs);
        }

        [HttpGet("{id:int}"), Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.ToPostDTO());
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePostRequestDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingTags = await _postRepository.GetAllTagsAsync();
            var postModel = postDTO.ToPostFromCreateDTO(existingTags);
            await _postRepository.CreateAsync(postModel);

            return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, postModel.ToPostDTO());
        }

        [HttpPut("{id:int}"), Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var postModel = await _postRepository.GetByIdAsync(id);
            var existingTags = await _postRepository.GetAllTagsAsync();

            if (postModel == null)
            {
                return NotFound();
            }

            postModel.UpdatePostFromDTO(updateDTO, existingTags);
            await _postRepository.UpdateAsync(postModel);

            return Ok(postModel.ToPostDTO());
        }

        [HttpDelete("{id:int}"), Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var postModel = await _postRepository.GetByIdAsync(id);

            if (postModel == null)
            {
                return NotFound();
            }

            await _postRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
