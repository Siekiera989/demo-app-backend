using System.ComponentModel.DataAnnotations;
using DemoApp.Attributes;

namespace DemoApp.Models.Request;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [PasswordComplexity]
    public string Password { get; set; }
}