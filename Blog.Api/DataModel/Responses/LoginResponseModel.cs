namespace Blog_Api.DataModel.Responses;

public class LoginResponseModel
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}