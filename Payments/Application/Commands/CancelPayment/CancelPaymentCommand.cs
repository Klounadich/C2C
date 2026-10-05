using MediatR;

namespace Payments.Application.Commands.CancelPayment;

public record CancelPaymentCommand(Guid OrderId,
    Guid UserId):IRequest<bool>;