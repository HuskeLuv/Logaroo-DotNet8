using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace post.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        [HttpGet, Authorize]
        public string Get()
        {
            return "Ok";
        }
    }
}