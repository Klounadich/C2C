using Payments.Application.Commands.CreatePayment;
using Payments.Application;
using Payments.Application.Commands.RefundPayment;
using Payments.Application.Commands.WebHook;
using Payments.Application.Queries;
using Payments.Controllers.Contracts;
using Payments.Domain;

namespace Payments.Application.Abstractions;

public interface IPaymentRepository
{
    public Task<bool> Create(Payment request, CancellationToken cancellationToken);
    
    public Task<GetPaymentResponce> Get(Guid paymentId, CancellationToken cancellationToken);
    
    public Task<CreatePaymentResponce> GetByIdempotencyKey (string idempotencyKey, CancellationToken cancellationToken);
    public Task<SetStatusResponce> SetStatus(PaymentWebhookRequest request);
    public Task<bool> Refund(RefundPaymentCommand request);
}