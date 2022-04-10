using System.Linq;
using Blog_Api.DataModel.Dtos;
using Blog_Api.DataModel.Entities;
using Blog_Api.Helpers;
using Blog_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly BlogContext _dbContext;
    private readonly IAuthService _authService;

    public UserService(BlogContext dbContext, IAuthService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task AddUser(RegisterUserRequestModel registerUserRequestModel)
    {
        var userEntity = new User
        {
            FirstName = registerUserRequestModel.FirstName,
            LastName = registerUserRequestModel.LastName,
            UserName = registerUserRequestModel.UserName,
            Email = registerUserRequestModel.Email,
            PasswordHash = EncryptionHelper.EncryptPassword(registerUserRequestModel.Password, registerUserRequestModel.Email),
            Role = "user",
            IsEmailConfirmed = false,
        };
        await _dbContext.Database.EnsureCreatedAsync();
        await _dbContext.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        await _authService.ConfirmEmail(userEntity.Id);
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