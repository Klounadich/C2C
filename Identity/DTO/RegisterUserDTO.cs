using System.ComponentModel.DataAnnotations;

namespace Identity.DTO;

public class RegisterUserDTO
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] [MinLength(6)] public string Username { get; set; }

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
};