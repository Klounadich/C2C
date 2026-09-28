using Microsoft.EntityFrameworkCore;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CreatePayment;
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
}