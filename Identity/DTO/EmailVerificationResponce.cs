namespace Identity.DTO;

public class EmailVerificationResponce
{
    public Guid UserId { get; set; }
    public bool CodeSent { get; set; }
}