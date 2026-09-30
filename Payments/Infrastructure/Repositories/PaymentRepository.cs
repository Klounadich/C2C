using Microsoft.EntityFrameworkCore;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CreatePayment;
using Payments.Application.Commands.RefundPayment;
using Payments.Application.Commands.WebHook;
using Payments.Application.Queries;
using Payments.Controllers.Contracts;
using Payments.Domain;
using Payments.Infrastructure.Persistence;

namespace Payments.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDBContext _dbContext;

    public PaymentRepository(PaymentDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async  Task<bool> Create(Payment request, CancellationToken cancellationToken)
    {
       await _dbContext.Payment.AddAsync(request);
       if (await _dbContext.SaveChangesAsync() > 0)
       {

          return true;
       }

       return false;
    }

    public async Task<SetStatusResponce> SetStatus(PaymentWebhookRequest request)
    {
        var payment = _dbContext.Payment.Where(x=> x.Id == request.PaymentId).SingleOrDefault();
        payment.Status = request.status;
        if (_dbContext.SaveChanges() > 0)
        {
            return new SetStatusResponce
            {
                orderId = payment.OrderId,
                status = request.status,
            };
        }
        return null;
    }

    public async  Task<CreatePaymentResponce?> GetByIdempotencyKey(string idempotencyKey, CancellationToken cancellationToken)
    {
        var payment =  await _dbContext.Payment
            .FirstOrDefaultAsync(
                x => x.IdempotencyKey == idempotencyKey,
                cancellationToken);
        if (payment != null)
        {
            return new CreatePaymentResponce(payment.Id,payment.ProviderPaymentId, "");

        }
        return null;
    }

    public async Task<GetPaymentResponce> Get(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await _dbContext.Payment.Where(x => x.Id == paymentId).FirstOrDefaultAsync();
        if (payment != null)
        {
            return new GetPaymentResponce(payment.Id,payment.OrderId,payment.Status,payment.Amount,payment.Currency,payment.UpdatedAt);
        }
        return null;
    }

    public async Task<bool> Refund(RefundPaymentCommand request)
    {
        var payment = await _dbContext.Payment.Where(x => x.OrderId == request.OrderId && x.UserId == request.UserId)
            .FirstOrDefaultAsync();
        payment.Status = PaymentStatus.RefundPending;
        payment.UpdatedAt = DateTime.UtcNow;
        return  await _dbContext.SaveChangesAsync() > 0;
        
    }
}