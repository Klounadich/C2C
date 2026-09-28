namespace Payments.Domain;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid OrderId { get;  set; }

    public Guid UserId { get;  set; }

    public decimal Amount { get;  set; }

    public string Currency { get; set; }

    public PaymentStatus Status { get; set; }

    public string Provider { get; set; }

    public string? ProviderPaymentId { get;  set; }

    public string IdempotencyKey { get;  set; }

    public DateTime CreatedAt { get; private set; } =  DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } =   DateTime.UtcNow;
}