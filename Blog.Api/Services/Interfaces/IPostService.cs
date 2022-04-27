using Blog_Api.DataModel.Entities;
using Blog_Api.DataModel.Requests;

namespace Blog_Api.Services.Interfaces;

public interface IPostService
{
    Task<Post?> ReadPost(Guid id);
    Task CreatePost(AddPostRequestModel post);
    Task UpdatePost(Post post);
    Task DeletePost(Guid id);
    Task<List<Post>> ReadAllPosts();
}