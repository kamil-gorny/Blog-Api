using Blog_Api.DataModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User>? Users { get; set; }
    
    public DbSet<Post?> Posts { get; set; }
}