using System.Text.RegularExpressions;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController:ControllerBase
{
    private readonly TokenService _tokenService;

    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] EmailService emailService,
        [FromServices] BlogDataContext context
    )
    {
        if(!ModelState.IsValid)
         return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

         var user = new User
         {
           Name = model.Name,
           Email = model.Email,
           Slug = model.Email.Replace("@","-").Replace(".","-")
         };
        var password = PasswordGenerator.Generate(
            length:25,includeSpecialChars:true,upperCase:false);

        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            

            emailService.Send(
                user.Name,
                user.Email,
                "Seja Bem vindo",
                $"Sua Senha é {password}"
            );
            
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,password
            }
            ));
        }catch(DbUpdateException)
        {
            return StatusCode(400,new ResultViewModel<string>("05X99 - Este E-mail já existe"));
        }catch(Exception e)
        {
            return StatusCode(500,"05X04 -ch Falha Interna " + e.Message);
        }
    }
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromServices] BlogDataContext context,
        [FromBody] LoginViewModel model
    )
    {
        if(!ModelState.IsValid) 
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors())); 
        
        User user = await context
        .Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(u => u.Email == model.Email);

        if(user == null) return StatusCode(401,"Usuário inválido");
        if(!PasswordHasher.Verify(user.PasswordHash,model.Password)) return StatusCode(401,"senha inválida");


        try
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token,null));
        }
        catch 
        {
            
            return BadRequest("50x04 -  Erro Interno");
        }
    }
    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] BlogDataContext context
    )
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg"; 
        var data = new Regex(@"^data:imageV[a-z]+;base64,")
        .Replace(model.Base64Image,"");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}",bytes);
        }
        catch (System.Exception)
        {
            return StatusCode(500,new ResultViewModel<string>("5x04 - Falha interna"));
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

        if(user == null) 
        return NotFound(new ResultViewModel<User>("Usuário não encontrado"));

        user.Image = $"http://localhost:5066/images/{fileName}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            return StatusCode(500,new ResultViewModel<string>("5x04 - Erro interno"));
        }

        return Ok(new ResultViewModel<string>("Imagem salva com sucesso!"));
    }

}