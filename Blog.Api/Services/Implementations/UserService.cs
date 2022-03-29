using System.Linq;
using Blog_Api.DataModel.Entities;
using Blog_Api.Helpers;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly BlogContext _dbContext;

    public UserService(BlogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUser(User user)
    {
        await _dbContext.Database.EnsureCreatedAsync();
        user.PasswordHash = EncryptionHelper.EncryptPassword(user.PasswordHash, user.Email);
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task RemoveUserById(Guid id)
    {
        var user = await GetUserById(id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditUser(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
}