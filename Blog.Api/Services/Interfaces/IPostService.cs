using Blog_Api.DataModel.Entities;

namespace Blog_Api.Services.Interfaces;

public interface IPostService
{ 
    Task<Post?> ReadPost(Guid id);
    Task CreatePost(Post post);
    Task UpdatePost(Post post);
    Task DeletePost(Guid id);
    Task<List<Post?>> ReadAllPosts();
}