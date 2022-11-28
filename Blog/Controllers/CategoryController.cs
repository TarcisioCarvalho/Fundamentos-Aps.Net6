using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController:ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context
        )
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            var category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.Id == id);

            if(category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel category,
            [FromServices] BlogDataContext context
        )
        {
           try
           {
            var categoryPost = new Category()
            {
                Id = 0,
                Name = category.Name,
                Slug = category.Slug.ToLower()
            };
            await context.Categories.AddAsync(categoryPost);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{categoryPost.Id}",categoryPost);
           }
           catch(DbUpdateException ex)
           {
            return StatusCode(500," EX09 Não foi Possível Incluir a Categoria");
           }
           catch (Exception ex)
           {
            
             return StatusCode(500," EX10 Erro Interno De Servidor");
           }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel category,
            [FromServices] BlogDataContext context
        )
        {
           var categoryPut = await context
           .Categories
           .FirstOrDefaultAsync(c => c.Id == id);

            if(categoryPut == null) return NotFound();

            categoryPut.Name = category.Name;
            categoryPut.Slug = category.Slug;

            context.Categories.Update(categoryPut);
            await context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("v1/categories/{id:int}")]

        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            var categoryDelete = await context
           .Categories
           .FirstOrDefaultAsync(c => c.Id == id);

           if(categoryDelete == null) return NotFound();

            context.Categories.Remove(categoryDelete);
            await context.SaveChangesAsync();

            return Ok(categoryDelete);
        }
    }
}