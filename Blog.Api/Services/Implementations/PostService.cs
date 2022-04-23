using Azure.Storage.Blobs;
using Blog_Api.DataModel.Entities;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class PostService : IPostService
{
    private readonly BlogContext _context;
    private readonly IConfiguration _configuration;

    public PostService(BlogContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<Post?> ReadPost(Guid id)
    {
        var result = await _context.Posts.Where(p => p.Id == id).Include(p => p.Comments).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result;
    }

    public async Task CreatePost(Post post)
    {
        var container = new BlobContainerClient(_configuration["BlobStorage:ConnectionString"], _configuration["BlobStorage:ContainerName"]);
        var blob = container.GetBlobClient("oki.jpg");
        
        var bytesFromBase64 = Convert.FromBase64String(post.ImageBase64);
        var streamContent = new StreamContent(new MemoryStream(bytesFromBase64));
        var stream = await streamContent.ReadAsStreamAsync();
        var blobInfo = await blob.UploadAsync(stream);
        
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