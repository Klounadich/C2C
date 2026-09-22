using Cart.Models;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure;

public class CartDBContext : DbContext
{
    public CartDBContext(DbContextOptions<CartDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<Carts> Carts { get; set; }
    public DbSet<CartItems> CartItems { get; set; }
}