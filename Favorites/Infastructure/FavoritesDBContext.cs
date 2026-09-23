using Favorites.Models;
using Microsoft.EntityFrameworkCore;

namespace Favorites.Infastructure;

public class FavoritesDBContext : DbContext
{
     public FavoritesDBContext(DbContextOptions<FavoritesDBContext> options) : base(options)
     {
          
     }
     public DbSet<FavoriteItems> FavoriteItems { get; set; }
}