namespace Identity.Models;

public class VerificationNotification
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Code_hashed { get; set; }
    public int Attempts { get; set; } = 0;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}