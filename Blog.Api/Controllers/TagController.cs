using Blog_Api.DataModel.Entities;
using Blog_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly BlogContext _context;
    
    public TagController(ITagService tagService, BlogContext context)
    {
        _tagService = tagService;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> PostTag(Tag tag)
    {
        await _tagService.AddTag(tag);
        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<List<Tag>>> GetTags()
    {
        try
        {
            var result = await _tagService.ReadAllTags();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTag(Guid id)
    {
        try
        {
            await _tagService.DeleteTag(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}