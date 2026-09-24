using Microsoft.EntityFrameworkCore;

namespace Payments.Infrastructure.Persistence;

public class PaymentDBContext : DbContext
{
    public PaymentDBContext(DbContextOptions<PaymentDBContext> options) : base(options)
    {
        
    }
}