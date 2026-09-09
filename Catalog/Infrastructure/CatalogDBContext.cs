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
}