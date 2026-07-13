using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IdentityDBContext : DbContext
{
    public IdentityDBContext(DbContextOptions<IdentityDBContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}