using Blog_Api.DataModel.Dtos;

namespace Blog_Api.Services.Interfaces;

public interface IAuthService
{
    Task<string> Login(LoginRequestModel loginRequestModel);
    Task ConfirmEmail(Guid id);
    Task SendConfirmationEmail(Guid id, string email);
}