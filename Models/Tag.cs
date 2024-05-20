
namespace app.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PostTag> postTags { get; set; }
    }
}