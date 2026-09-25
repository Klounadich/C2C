using Microsoft.EntityFrameworkCore;
using Order.Domain;

namespace Order.Infrastructure.Persistance;

public class OrderDBContext : DbContext
{
    public OrderDBContext(DbContextOptions<OrderDBContext> options) : base(options)
    {
        
    }
    public DbSet<Orders>  Orders { get; set; }
}