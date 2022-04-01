namespace Blog_Api.Services.Interfaces;

public interface IAuthService
{
    Task<string> Login(string email, string password);
    Task ConfirmEmail(Guid id);
    Task SendConfirmationEmail(Guid id, string email);
}