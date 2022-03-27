using Blog_Api.DataModel.Entities;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class TagService : ITagService
{
    private readonly BlogContext _context;

    public TagService(BlogContext context)
    {
        _context = context;
    }

    public async Task<Tag?> ReadTag(Guid id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task DeleteTag(Guid id)
    {
        var result = await ReadTag(id);
        _context.Tags.Remove(result);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tag>> ReadAllTags()
    {
        return await _context.Tags.ToListAsync();
    }
}