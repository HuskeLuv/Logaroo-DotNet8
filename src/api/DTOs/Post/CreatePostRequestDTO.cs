
using System.ComponentModel.DataAnnotations;

namespace app.DTOs.Post
{
    public class CreatePostRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        [MaxLength(150, ErrorMessage = "Title must be at max 150 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Author name must be at least 3 characters")]
        [MaxLength(150, ErrorMessage = "Author name must be at max 150 characters")]
        public string Author { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Content must be at least 3 characters")]
        public string Content { get; set; } = string.Empty;
        [Required]
        public List<string> Tags { get; set; } = new List<string>();
    }
}