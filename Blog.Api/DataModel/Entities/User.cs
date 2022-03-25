using System.ComponentModel.DataAnnotations;

namespace Blog_Api.DataModel.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    [EmailAddress] 
    public string Email { get; set; }
    public string? PasswordHash { get; set; }
    public bool? IsEmailConfirmed { get; set; }
}