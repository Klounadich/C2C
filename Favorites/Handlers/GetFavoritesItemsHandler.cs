using Favorites.DTOs;
using Favorites.Repositories;
using MediatR;

namespace Favorites.Handlers;

public class GetFavoritesItemsHandler : IRequestHandler<GetFavoritesItemsDTO ,FavoritesItemsResponce>
{
    private readonly IFavoritesRepository _favoritesRepository;

    public GetFavoritesItemsHandler(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }
    public async Task<FavoritesItemsResponce> Handle(GetFavoritesItemsDTO request, CancellationToken cancellationToken)
    {
       return await  _favoritesRepository.GetFavoritesAsync(request.UserId, request.page, request.pageSize);
    }
}