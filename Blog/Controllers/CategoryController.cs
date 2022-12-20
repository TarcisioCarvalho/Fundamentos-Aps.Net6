using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController:ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromServices] IMemoryCache cache
        )
        {
            try
            {
                var categories = await cache.GetOrCreateAsync("CategoriesCache",entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return  context.Categories.ToListAsync();
                });

                return Ok(new ResultViewModel <List<Category>>(categories));
            }
            catch 
            {
                return StatusCode(500,new ResultViewModel<List<Category>>("5x00 - Falha Interna no Servidor"));
            }
            
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            try
            {
                 var category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.Id == id);

            if(category == null) return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

            return Ok(new ResultViewModel<Category>(category));
            }
            catch 
            {
                return StatusCode(500, "Falha Interna no Servidor");
            }
           

            
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel category,
            [FromServices] BlogDataContext context
        )
        {
            if(!ModelState.IsValid) return BadRequest(
                new ResultViewModel<Category>(ModelState.GetErrors())
            );
            
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

            return Created($"v1/categories/{categoryPost.Id}",new ResultViewModel<Category>(categoryPost));
           }
           catch(DbUpdateException)
           {
            return StatusCode(500,new ResultViewModel<Category>("EX09 Não foi Possível Incluir a Categoria"));
           }
           catch (Exception)
           {
             return StatusCode(500,new ResultViewModel<Category>("EX10 Erro Interno De Servidor"));
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

            if(categoryPut == null) return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

            categoryPut.Name = category.Name;
            categoryPut.Slug = category.Slug;

            context.Categories.Update(categoryPut);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(categoryPut));
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

           if(categoryDelete == null) return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

            context.Categories.Remove(categoryDelete);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(categoryDelete));
        }
    }
}