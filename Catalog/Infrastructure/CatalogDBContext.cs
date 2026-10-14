using Catalog.Models;
using Catalog.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Infrastructure;

public class CatalogDBContext : DbContext
{
    public CatalogDBContext(DbContextOptions<CatalogDBContext> options): base(options)
    {
        
    }
    public DbSet<Items> Items  { get; set; }
    public DbSet<ItemAlias> Aliases  { get; set; }
    public DbSet<SearchLogs>  SearchLogs { get; set; } 
    public DbSet<Reports> Reports { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Items>(entity =>
        {
            entity.Property(x => x.Location)
                .HasColumnType("geography(Point,4326)");

            entity.HasIndex(x => x.Location)
                .HasMethod("gist");
        });
    }

}