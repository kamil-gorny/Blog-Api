using Blog_Api.DataModel.Entities;

namespace Blog_Api.Services.Interfaces;

public interface ICommentService
{
    Task<Comment?> ReadComment(Guid id);
    Task CreateComment(Comment post);
    Task UpdateComment(Comment comment);
    Task DeleteComment(Guid id);
    Task<List<Comment>> ReadAllCommentsForPost(Guid postId);
}