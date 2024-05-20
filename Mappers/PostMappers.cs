using System.Text.Json;
using app.DTOs.Post;
using app.Models;
using post.Models;

namespace app.Mappers
{
    public static class PostMappers
    {
        public static PostDTO ToPostDTO(this Post postModel)
        {
            return new PostDTO
            {
                Id = postModel.Id,
                Title = postModel.Title,
                Author = postModel.Author,
                Content = postModel.Content,
                Tags = postModel.PostTags.Select(pt => pt.Tag.Name).ToList(),
                Created_At = postModel.Created_At,
                Updated_At = postModel.Updated_At
            };
        }

        public static Post ToPostFromCreateDTO(this CreatePostRequestDTO postDTO, List<Tag> existingTags)
        {
            return new Post
            {
                Title = postDTO.Title,
                Author = postDTO.Author,
                Content = postDTO.Content,
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now,
                PostTags = postDTO.Tags.Select(tagName =>
                {
                    var tag = existingTags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName };
                    return new PostTag { Tag = tag };
                }).ToList()
            };
        }

        public static void UpdatePostFromDTO(this Post postModel, UpdatePostRequestDTO postDTO, List<Tag> existingTags)
        {
            postModel.Title = postDTO.Title;
            postModel.Author = postDTO.Author;
            postModel.Content = postDTO.Content;
            postModel.Updated_At = DateTime.Now;

            // Atualize as tags
            postModel.PostTags.Clear();
            foreach (var tagName in postDTO.Tags)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName };
                postModel.PostTags.Add(new PostTag { PostId = postModel.Id, Tag = tag });
            }
        }
    }
}