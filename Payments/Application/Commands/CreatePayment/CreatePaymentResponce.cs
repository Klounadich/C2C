namespace Payments.Application.Commands.CreatePayment;

public record CreatePaymentResponce(
    Guid? PaymentId,
    string? PaymentUrl,
    string? ErrorMessage);