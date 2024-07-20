using System.ComponentModel.DataAnnotations;

namespace LeafLINQWebAPI.DTOs;

public class LoginDTO
{
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a Valid Email Please"),
    StringLength(50), Display(Name = "Email"), Required]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
