using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Identity.DTO;

public class AuthUserDTO 
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required]
    public string Password { get; set; }

   
}