using Microsoft.EntityFrameworkCore;
using Order.Domain;

namespace Order.Infrastructure.Persistance;

public class OrderDBContext : DbContext
{
    public OrderDBContext(DbContextOptions<OrderDBContext> options) : base(options)
    {
        
    }
    public DbSet<Orders>  Orders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.Property(x => x.deliveryLocation)
                .HasColumnType("geography(Point,4326)");

            entity.HasIndex(x => x.deliveryLocation)
                .HasMethod("gist");
        });
    }
}