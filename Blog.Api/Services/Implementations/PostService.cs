using Blog_Api.DataModel.Entities;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class PostService : IPostService
{
    private readonly BlogContext _context;

    public PostService(BlogContext context)
    {
        _context = context;
    }

    public async Task<Post?> ReadPost(Guid id)
    {
        var result = await _context.Posts.FindAsync(id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result;
    }

    public async Task CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePost(Guid id)
    {
        var result = await ReadPost(id);
        _context.Posts.Remove(result);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Post>> ReadAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }
}