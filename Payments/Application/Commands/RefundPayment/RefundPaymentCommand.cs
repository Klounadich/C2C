using MediatR;

namespace Payments.Application.Commands.RefundPayment;

public record RefundPaymentCommand(
    Guid OrderId,
    Guid UserId):IRequest<bool>;