using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O Campo nome é obrigatório")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O Campo Email é obrigatório")]
    [EmailAddress]
    public string Email { get; set; }
}