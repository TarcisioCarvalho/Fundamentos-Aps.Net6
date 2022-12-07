using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
   
    public class HomeController:ControllerBase
    {
        
        [HttpGet("")]
         [ApiKey]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}