using Blog_Api.DataModel.Dtos;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserRequestModel createUserRequestModel)
    {
        try
        {
            await _userService.AddUser(createUserRequestModel);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var result = await _authService.Login(email, password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}