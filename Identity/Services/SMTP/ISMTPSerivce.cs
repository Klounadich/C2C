using Identity.Services.SMTP.Models;

namespace Identity.Services.SMTP;

public interface ISMTPSerivce
{
    public Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}