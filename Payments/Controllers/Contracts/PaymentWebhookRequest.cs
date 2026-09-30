using MediatR;
using Payments.Domain;

namespace Payments.Controllers.Contracts;

public record PaymentWebhookRequest(
    Guid PaymentId,
    PaymentStatus status):IRequest<bool>;