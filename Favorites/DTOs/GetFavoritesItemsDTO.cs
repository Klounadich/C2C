using MediatR;

namespace Favorites.DTOs;

public class GetFavoritesItemsDTO: IRequest<FavoritesItemsResponce>
{
   public int page { get; set; }
    public int pageSize { get; set; }
     public Guid UserId { get; set; }
}