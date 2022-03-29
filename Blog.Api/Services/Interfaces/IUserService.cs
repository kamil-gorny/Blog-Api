using Blog_Api.DataModel.Entities;

namespace Blog_Api.Services.Interfaces;

public interface IUserService
{
    Task AddUser(User user);
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByEmail(string email);
    Task RemoveUserById(Guid id);
    Task EditUser(User user);
    Task<List<User>> GetUsers();
}