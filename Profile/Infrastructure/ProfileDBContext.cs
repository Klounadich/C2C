using Microsoft.EntityFrameworkCore;
using Profile.Models;

namespace Profile.Infrastructure;

public class ProfileDBContext : DbContext
{
    public ProfileDBContext(DbContextOptions<ProfileDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<Items> Items  { get; set; }
}