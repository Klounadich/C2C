namespace Payments.Domain;

public class Payment
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid UserId { get; private set; }

    public long Amount { get; private set; }

    public string Currency { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string Provider { get; private set; }

    public string? ProviderPaymentId { get; private set; }

    public string IdempotencyKey { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
}