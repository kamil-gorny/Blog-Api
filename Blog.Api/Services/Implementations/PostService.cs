using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Storage.Blobs;
using Blog_Api.DataModel.Entities;
using Blog_Api.DataModel.Requests;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class PostService : IPostService
{
    private readonly BlogContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PostService(BlogContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
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

    public async Task CreatePost(AddPostRequestModel postRequest)
    {
        var container = new BlobContainerClient(_configuration["BlobStorage:ConnectionString"], _configuration["BlobStorage:ContainerName"]);
        var imageName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jpg");
        var blob = container.GetBlobClient($"{imageName}");
    
        var image = Regex.Replace(postRequest.ImageBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
        var bytesFromBase64 = Convert.FromBase64String(image);
        var streamContent = new StreamContent(new MemoryStream(bytesFromBase64));
        var stream = await streamContent.ReadAsStreamAsync();
        
        await blob.UploadAsync(stream);
        var post = _mapper.Map<Post>(postRequest);
        
        post.ImageUrl = $"{_configuration["BlobStorage:Url"]}/{_configuration["BlobStorage:ContainerName"]}/{imageName}";
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