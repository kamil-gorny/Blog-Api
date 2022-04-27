using Microsoft.AspNetCore.Mvc;
using Blog_Api.DataModel.Entities;
using Blog_Api.DataModel.Requests;
using Blog_Api.Exceptions;
using Blog_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Blog_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            try
            {
                var result = await _postService.ReadPost(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            try
            {
                var result = await _postService.ReadAllPosts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        
        [HttpPut]
        public async Task<IActionResult> PutPost(Post post)
        {
            try
            {
                await _postService.UpdatePost(post);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostPost(AddPostRequestModel post)
        {
            try
            {
                await _postService.CreatePost(post);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                await _postService.DeletePost(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        
    }
}
