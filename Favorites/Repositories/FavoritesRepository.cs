using Favorites.DTOs;
using Favorites.Infastructure;
using Favorites.Models;
using Microsoft.EntityFrameworkCore;

namespace Favorites.Repositories;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly FavoritesDBContext _context;
    public FavoritesRepository(FavoritesDBContext context)
    {
        _context = context;
    }
    public async Task<bool> SameItemAsync(
        Guid userId,
        Guid itemId)
    {
        if (!_context.FavoriteItems.Any(x => x.ItemId == itemId && x.UserId == userId))
        {
            return false;
        }
        return true;
    }

    public async Task<bool> AddFavoriteAsync(
        Guid userId,
        Guid itemId)
    {
        
        await _context.FavoriteItems.AddAsync(new FavoriteItems{UserId = userId , ItemId = itemId});
        return await _context.SaveChangesAsync() > 0;
    }

  public async Task<FavoritesItemsResponce> GetFavoritesAsync(Guid userId)
  {
      var items = new List<Guid>();
      items = await _context.FavoriteItems.Where(x => x.UserId == userId).Select(x=>x.ItemId).ToListAsync();
      return new FavoritesItemsResponce
      {
          itemsId = items
      };
  }

  public async Task<bool> RemoveFavoriteAsync(Guid userId,
      Guid itemId)
  {
      _context.FavoriteItems
  }
}