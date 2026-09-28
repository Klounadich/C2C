using MediatR;

namespace Payments.Application.Commands.CreatePayment;

public record CreatePaymentCommand(
    Guid OrderId,
    Guid UserId,
    string idempotencyKey):IRequest<CreatePaymentResponce>;