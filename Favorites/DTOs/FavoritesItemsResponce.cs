using MediatR;

namespace Favorites.DTOs;

public class FavoritesItemsResponce 
{
    public List<Guid> itemsId {get; set;}
}