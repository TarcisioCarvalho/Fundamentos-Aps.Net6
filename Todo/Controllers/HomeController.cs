using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
      
        public IActionResult Get([FromServices] AppDbContext context)
        =>  Ok(context.Todos.ToList());
        

          [HttpGet("/{id:int}")]
      
        public IActionResult GetById([FromRoute] int id,[FromServices] AppDbContext context)
        {
            var todo = context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPost("/")]
         public IActionResult Post([FromBody]TodoModel todo,[FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();
            return Created($"/{todo.Id}",todo);
        }

        [HttpPut("/{id:int}")]

        public IActionResult Put(
            [FromRoute] int id,
            [FromBody]TodoModel todo,
            [FromServices] AppDbContext context
        )
        {
            var todoPut = context.Todos.FirstOrDefault(t => t.Id == id);
            if (todoPut == null) return NotFound();

            todoPut.Title = todo.Title;
            todoPut.Done = todo.Done;
            context.Todos.Update(todoPut);
            context.SaveChanges();
            return Ok(todoPut);
        }

          [HttpDelete("/{id:int}")]

        public IActionResult Delete(
            [FromRoute] int id,
            [FromServices] AppDbContext context
        )
        {
            var todoDelete = context.Todos.FirstOrDefault(t => t.Id == id);
            if(todoDelete == null) return NotFound();
            context.Todos.Remove(todoDelete);
            context.SaveChanges();
            return Ok(todoDelete);
        }
    }
}