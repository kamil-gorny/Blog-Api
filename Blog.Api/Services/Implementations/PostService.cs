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
        
        var bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAArwAAAGXCAQAAAD7MEugAAAEr0lEQVR42u3dL2hVYRzH4a/zKpNNhImCwyRsBotOwWQwDSxG0WSyWSyCCktmYSaDomFJwSaKIIIgbPivimAQuf6B6VDEDaezmd/Xcbj3Hp5n9cfh3N9574dxy0kAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPrChoauuzHDlgv0oZWs9voWOg1ddyrXPF+gD13I/baGdzQHPF+gD431/haGPAUA4QUQXgCEF0B4ARBeAOEFQHgBhBdAeAEQXgDhBUB4AYQXAOEFEF4A4QVAeAGEF4AaHSvgn69ZKp7dkdHi2Q9Z7vEn25xdxf9k/MzH4uuOZKdjg/CyHrdypXh2NseLZ8/laY8/2b7czkjh7MucLL7u8cw6Nggv67GUd8WzPyqu+6nius0Yy1rx7ErF3S46NPwPv/ECCC+A8AIgvADCC4DwAggvAMILILwAwguA8AIILwDCCyC8AAgvgPACCC8AwgsgvABUaeqda9/zrHh2LHs8iMZ8yduK6UMVT62ttlZsYVvFOR/PuOPYmG66Fd+J1ob3RY4Uz57ITeemMQ9zunj2fJ4Uz25q7camKrZwp+KcX8wlx7ExN3K5ePZXe8P7J8uDtIYW+13xJJJhC8tQxRaGKra7arUNWq06531wyAAQXgDhBUB4AYQXAOEFEF4AhBdAeAGEFwDhBRBeAIQXQHgBEF4A4QUQXgCEF0B4AajQ6YN76OZe8exEJjy0LGa+ePZzjhXPTlbcw6uKt7ou9nxj3/IgWwpnt+dw8XXHK7abinO+3xuJk3Tzqnj2jXU1aSZr/vKoYmOnGrqHU609Y0cb2thMxT3MOeVZy5yfGgAQXgDhBUB4AYQXQHitAEB4AYQXAOEFEF4AhBdAeAEQXgDhBRBeAIQXQHgBEF4A4QVAeAF6oTNg97uQq8Wz01VvzR0ku3O2ePZgxXXns1A8+7q134n3FWdsMtPFs4crntreFm/3bvHsc4EeRN7U2uR7cEmae4ezN2T7qQEA4QUQXgCEF0B4ARBeAOEFEF4AhBdAeAEQXgDhBUB4AYQXQHgBEF4A4QVAeAGEFwDhBeiZTos/2/U89oCrvLCCSgs5YwlVulYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAArfUXZnXds+Lqx9UAAAAASUVORK5CYII=");
        var contents = new StreamContent(new MemoryStream(bytes));
        var stream = await contents.ReadAsStreamAsync();
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