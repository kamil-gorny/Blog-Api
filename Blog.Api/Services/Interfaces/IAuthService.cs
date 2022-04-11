using Blog_Api.DataModel.Dtos;
using Blog_Api.DataModel.Responses;

namespace Blog_Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel);
    Task ConfirmEmail(Guid id);
    Task SendConfirmationEmail(Guid id, string email);
}