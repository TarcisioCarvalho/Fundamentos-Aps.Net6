using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;



[ApiController]
public class PostController: ControllerBase
{
    [HttpGet("v1/Posts")]
    public async Task<IActionResult> GetPosts(
        [FromServices] BlogDataContext context
    )
    {
        try
        {
            var posts = await context.Posts.ToListAsync();
            return Ok(new ResultViewModel<List<Post>>(posts));
        }
        catch 
        {
            return StatusCode(500,"50x04 - falha interna");
        }
    }
}