namespace Payments.Controllers.Contracts;

public record CreatePaymentRequest(
    Guid OrderId,
    string idempotencyKey);