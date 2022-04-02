using Blog_Api.DataModel.Dtos;
using Blog_Api.DataModel.Entities;

namespace Blog_Api.Services.Interfaces;

public interface IUserService
{
    Task AddUser(RegisterUserRequestModel user);
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByEmail(string email);
    Task RemoveUserById(Guid id);
    Task EditUser(User user);
    Task<List<User>> GetUsers();
}