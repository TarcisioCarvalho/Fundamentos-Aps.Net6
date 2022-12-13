using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Campo obrigatório")]
    [EmailAddress(ErrorMessage = "Digite no formato de email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public string Password { get; set; }
}