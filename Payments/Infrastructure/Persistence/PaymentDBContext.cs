using Microsoft.EntityFrameworkCore;
using Payments.Domain;

namespace Payments.Infrastructure.Persistence;

public class PaymentDBContext : DbContext
{
    public PaymentDBContext(DbContextOptions<PaymentDBContext> options) : base(options)
    {
        
    }
    public DbSet<Payment> Payment { get; set; }
    
}