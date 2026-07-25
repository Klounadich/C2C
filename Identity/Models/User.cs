namespace Identity.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public float? Rating { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description {get; set;}
    public DateTime CreatedAt { get; set; }
    
    
}