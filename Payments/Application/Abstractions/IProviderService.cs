namespace Payments.Application.Abstractions;

public interface IProviderService
{
    public Task<string> SendPayment(Guid paymentId,
        decimal amount,
        string currency,
        string description);
}