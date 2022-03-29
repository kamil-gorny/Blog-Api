using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Blog_Api.DataModel;
using Blog_Api.DataModel.Entities;
using Blog_Api.Helpers;
using Blog_Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Blog_Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly BlogContext _dbContext;

        public AuthService(IUserService userService, IConfiguration configuration, BlogContext dbContext)
        {
            _userService = userService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        private const double ExpiryDurationMinutes = 100;
        public async Task<string> GetToken(string email, string password)
        {
            var user = await _userService.GetUserByEmail(email);
            if(user.PasswordHash.Equals(EncryptionHelper.EncryptPassword(password, email)))
            {
                return GenerateToken(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], user);
            }

            return string.Empty;
        }

        private string GenerateToken(string key, string issuer, User user)
        {
            var claims= new ClaimsIdentity(new[]
            {
                new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
            });
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);  
            
            var tokenDescriptor = tokenHandler.CreateJwtSecurityToken(issuer, issuer, claims, 
                expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes), signingCredentials: credentials); 
            
            var results = new
            {
                token = tokenHandler.WriteToken(tokenDescriptor),
                expiration = tokenDescriptor.ValidTo,
            };
            return results.token;
        }

        public async Task SendConfirmationEmail(Guid id, string email)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_configuration["SMTP:Email"] ,_configuration["SMTP:EmailPassword"]),
                EnableSsl = true,
            };
    
            smtpClient.Send(_configuration["SMTP:Email"], email, "Confirmation Email - kamilgorny.me", $"Here is your confirmation link: https://localhost:5001/api/Auth/{id}");
        }

        public async Task ConfirmEmail(Guid id)
        {
            var user = await _userService.GetUserById(id);
            user.IsEmailConfirmed = true;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }
}