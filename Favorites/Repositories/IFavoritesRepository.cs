using Favorites.DTOs;

namespace Favorites.Repositories;

public interface IFavoritesRepository
{
    Task<bool> SameItemAsync(
        Guid userId,
        Guid itemId);
    
    Task<bool> AddFavoriteAsync(
        Guid userId,
        Guid itemId);

    Task<bool> RemoveFavoriteAsync(Guid userId,
        Guid itemId);
    
    Task<FavoritesItemsResponce> GetFavoritesAsync(Guid userId ,int page,
        int pageSize);
}