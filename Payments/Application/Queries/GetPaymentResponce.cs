using Payments.Domain;

namespace Payments.Application.Queries;

public record GetPaymentResponce(
    Guid PaymentId,
    Guid OrderId,
    PaymentStatus PaymentStatus,
    decimal Amount,
    string Currency,
    DateTime UpdatedAt);