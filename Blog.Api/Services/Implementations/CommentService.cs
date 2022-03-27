using Blog_Api.DataModel.Entities;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly BlogContext _context;

    public CommentService(BlogContext context)
    {
        _context = context;
    }

    public async Task<Comment?> ReadComment(Guid id)
    {
        var result = await _context.Comments.FindAsync(id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result;
    }

    public async Task CreateComment(Comment post)
    {
        await _context.Comments.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateComment(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid id)
    {
        var result = await ReadComment(id);
        _context.Comments.Remove(result);
        await _context.SaveChangesAsync();
    }
    
}