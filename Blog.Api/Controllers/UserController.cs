using Blog_Api.DataModel.Dtos;
using Blog_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> PostUser(RegisterUserRequestModel user)
    {
        try
        {
            await _userService.AddUser(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
            
    }
    
}