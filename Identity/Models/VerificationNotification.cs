namespace Identity.Models;

public class VerificationNotification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Code_hashed { get; set; }
    public int Attempts { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}